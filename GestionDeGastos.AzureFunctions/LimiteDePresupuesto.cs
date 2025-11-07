using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace GestionDeGastos.AzureFunctions
{
    public class PresupuestoData
    {
        public int PresupuestoId { get; set; }
        public int UsuarioId { get; set; }
        public string Email { get; set; }
        public decimal MontoLimite { get; set; }
        public decimal MontoActualGastado { get; set; }
    }

    public class AlertaEntity : ITableEntity //modelo para mandar al storage
    {
        public string PartitionKey { get; set; } = "Presupuestos";
        public string RowKey { get; set; } // combinación de UsuarioId+PresupuestoId
        public DateTimeOffset? SentAt { get; set; }
        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
    }

    public class LimiteDePresupuesto
    {
        private readonly ILogger<LimiteDePresupuesto> _logger;
        private const decimal ALERT_THRESHOLD = 0.85m; // cuando llegue al 85%
        private const int COOLDOWN_HOURS = 12;         // no reenviar dentro de 12h

        public LimiteDePresupuesto(ILogger<LimiteDePresupuesto> logger)
        {
            _logger = logger;
        }

        [Function("LimiteDePresupuesto")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("Procesando límite de presupuesto...");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                _logger.LogInformation($"Request body: {requestBody}");

                var data = JsonSerializer.Deserialize<PresupuestoData>(requestBody);
                if (data == null)
                {
                    _logger.LogError("Error: el cuerpo de la solicitud no se pudo deserializar correctamente.");
                    return new BadRequestObjectResult("Datos inválidos o incompletos.");
                }

                _logger.LogInformation($"Email: {data.Email}, MontoLimite: {data.MontoLimite}, Gastado: {data.MontoActualGastado}");

                if (data.MontoLimite <= 0 || string.IsNullOrWhiteSpace(data.Email))
                {
                    return new BadRequestObjectResult("Datos inválidos o incompletos.");
                }

                decimal ratio = data.MontoActualGastado / data.MontoLimite;
                if (ratio < ALERT_THRESHOLD)
                {
                    _logger.LogInformation($"Monto aún debajo del umbral ({ratio:P0}).");
                    return new OkObjectResult(new { sent = false, message = "Sin alerta" });
                }

                string storageConn = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                _logger.LogInformation($"Usando Storage: {(string.IsNullOrEmpty(storageConn) ? "NULO" : "OK")}");

                var tableClient = new TableClient(storageConn, "AlertasPresupuesto");
                await tableClient.CreateIfNotExistsAsync();

                string rowKey = $"{data.UsuarioId}_{data.PresupuestoId}";
                var existing = await tableClient.GetEntityIfExistsAsync<AlertaEntity>("Presupuestos", rowKey);

                if (existing.HasValue && existing.Value.SentAt.HasValue &&
                    existing.Value.SentAt.Value > DateTimeOffset.UtcNow.AddHours(-COOLDOWN_HOURS))
                {
                    _logger.LogInformation($"Ya se envió alerta en las últimas {COOLDOWN_HOURS}h.");
                    return new OkObjectResult(new { sent = false, message = "Alerta ya enviada recientemente." });
                }

                string sendGridKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
                if (string.IsNullOrEmpty(sendGridKey))
                {
                    _logger.LogError("Falta la variable SENDGRID_API_KEY.");
                    return new BadRequestObjectResult("Falta configuración SENDGRID_API_KEY.");
                }

                _logger.LogInformation("Preparando envío de correo...");
                var client = new SendGridClient(sendGridKey);
                var from = new EmailAddress("carla.stram@gmail.com", "Gestión de Gastos");
                var subject = "Estás cerca de tu límite de presupuesto mensual";
                var to = new EmailAddress(data.Email);
                var plainTextContent = $"Has gastado {ratio:P0} de tu presupuesto actual.";
                var htmlContent = $"<strong>¡Cuidado!</strong> Has gastado el {ratio:P0} de tu presupuesto actual. Revisa tus gastos.";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

                var response = await client.SendEmailAsync(msg);
                _logger.LogInformation($"Respuesta SendGrid: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Fallo al enviar correo: {response.StatusCode}");
                    return new BadRequestObjectResult("Error al enviar el correo.");
                }

                var entity = new AlertaEntity
                {
                    RowKey = rowKey,
                    SentAt = DateTimeOffset.UtcNow
                };
                await tableClient.UpsertEntityAsync(entity);

                _logger.LogInformation("Correo de alerta enviado correctamente.");
                return new OkObjectResult(new { sent = true, message = "Correo de alerta enviado correctamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error inesperado: {ex.Message}");
                _logger.LogError(ex.StackTrace);
                return new StatusCodeResult(500);
            }
        }
    }
}
