using Azure;
using Azure.AI.DocumentIntelligence;
using GestionDeGastos.Servicio.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeGastos.Servicio
{
    public interface IDocumentIntelligenceServicio
    {
        Task<TicketEscaneadoDTO> EscanearTicketAsync(IFormFile ticketArchivo);
    }
    public class DocumentIntelligenceServicio : IDocumentIntelligenceServicio
    {
        private readonly DocumentIntelligenceClient _cliente;
        public DocumentIntelligenceServicio(IConfiguration configuration)
        {
            string endpoint = configuration["DocumentIntelligence:Endpoint"];
            string apiKey = configuration["DocumentIntelligence:ApiKey"];

            if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("El endpoint o la clave de Document Intelligence no están configurados.");
            }

            var credential = new AzureKeyCredential(apiKey);
            _cliente = new DocumentIntelligenceClient(new Uri(endpoint), credential);
        }
        public async Task<TicketEscaneadoDTO> EscanearTicketAsync(IFormFile ticketArchivo)
        {
            await using var stream = new MemoryStream();
            await ticketArchivo.CopyToAsync(stream);
            stream.Position = 0;

            var binary = BinaryData.FromStream(stream);

            // Usamos el modelo pre-entrenado de recibos ("prebuilt-receipt")
            Operation<AnalyzeResult> operation = 
                await _cliente.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-receipt", binary);

            AnalyzeResult result = operation.Value;

            // Mapear la respuesta de Azure a el DTO que devolvera el endpoint AJAX
            return ParsearResultado(result);
        }

        private TicketEscaneadoDTO ParsearResultado(AnalyzeResult result)
        {
            var ticket = new TicketEscaneadoDTO();

            var doc = result.Documents.FirstOrDefault();
            if (string.IsNullOrEmpty(doc?.DocumentType) || !doc.DocumentType.Contains("receipt", StringComparison.OrdinalIgnoreCase))
                return ticket;

            // Total analizar si hace falta
            //if (doc.Fields.TryGetValue("Total", out DocumentField totalField))
            //{
            //    var moneda = totalField.ValueCurrency;
            //    if (moneda != null)
            //        ticket.MontoTotal = (decimal)moneda.Amount;
            //}

            // Fecha
            var fecha = doc.Fields.TryGetValue("TransactionDate", out var dateField)
                        ? dateField.ValueDate
                        : null;

            if (fecha.HasValue)
                ticket.FechaEscaneada = DateOnly.FromDateTime(fecha.Value.Date);


            // Total 
            decimal totalLimpio = 0;
            if (doc.Fields.TryGetValue("Total", out var total))
                totalLimpio = (decimal?)total.ValueCurrency?.Amount ?? 0;

            // Subtotal
            decimal subtotalLimpio = 0;
            if (doc.Fields.TryGetValue("Subtotal", out var subtotal))
                subtotalLimpio = (decimal?)subtotal.ValueCurrency?.Amount ?? 0;

            decimal diferencia = totalLimpio - subtotalLimpio;

            if (diferencia > 0)
            {
                // Si la diferencia es positiva, la consideramos como IVA
                ticket.Iva = diferencia;
            }
            else if (diferencia < 0)
            {
                // Si la diferencia es negativa, la consideramos como Descuento
                ticket.Descuento = diferencia;// considerar el Abs
            }

            // Items
            if (doc.Fields.TryGetValue("Items", out var itemsField) && itemsField.ValueList != null)
            {
                foreach (var itemField in itemsField.ValueList)
                {
                    var obj = itemField.ValueDictionary;
                    if (obj == null) continue; // si no encuentra el diccionario de valores, seguimos con el siguiente item

                    var descripcion = obj.TryGetValue("Description", out var desc) 
                                        ? desc.ValueString 
                                        : null;

                    var cantidadDouble = obj.TryGetValue("Quantity", out var qty) 
                                    ? (qty.ValueDouble ?? 1.0) 
                                    : 1.0;
                    // si consigue 0, le asignamos 1 a cantidad de item para mantener la logica y no romper la division
                    if (cantidadDouble == 0) cantidadDouble = 1.0; 

                    var precioTotal = obj.TryGetValue("TotalPrice", out var price) ? (decimal?)(price.ValueCurrency?.Amount ?? 0) : null;

                    var precioUnitario = obj.TryGetValue("Price", out var unitPrice) ? (decimal?)(unitPrice.ValueCurrency?.Amount ?? 0) : null;

                    if (precioUnitario == null && precioTotal != null)
                        // Si no vino el precio unitario, lo calculamos con los valores que si conseguimos
                        precioUnitario = precioTotal / (decimal)cantidadDouble;
                    

                    if (!string.IsNullOrWhiteSpace(descripcion))
                    {
                        ticket.ItemsEscaneados.Add(new TicketEscaneadoItemDTO
                        {
                            Descripcion = descripcion,
                            Cantidad = (int)cantidadDouble,
                            PrecioUnitario = precioUnitario ?? 0
                        });
                    }
                }
            }

            return ticket;
        }
    }
}
