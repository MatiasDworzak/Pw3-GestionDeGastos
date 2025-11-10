using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

namespace GestionDeGastos.Servicio
{
    public interface ILimiteDePresupuestoServicio
    {
        Task<string> EnviarAlertaSiCorrespondeAsync(int presupuestoId, int usuarioId, string email,
        decimal montoLimite, decimal montoActualGastado);
    }
    public class LimiteDePresupuestoServicio : ILimiteDePresupuestoServicio
    {
        private readonly IHttpClientFactory _httpClientFactory; // Inyección de dependencia para el cliente HTTP que hace llamadas a la web
        private readonly IConfiguration _configuration; // Permite acceder a valores como la URL y clave de azure functions

        public LimiteDePresupuestoServicio(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<string> EnviarAlertaSiCorrespondeAsync(int presupuestoId, int usuarioId, string email,
       decimal montoLimite, decimal montoActualGastado)
        {
            var client = _httpClientFactory.CreateClient("Functions");

            var urlFunction = _configuration["AzureFunction:Url"];
            /*
            Cuando este deployado usar
            var urlFunction = Environment.GetEnvironmentVariable("LimiteDePresupuestoAzureFunction");
            */
            /*
            o tmb
            var urlFunction =
            Environment.GetEnvironmentVariable("LimiteDePresupuestoAzureFunction")
            ?? _configuration["AzureFunction:Url"];
            */
            // var keyFunction = _configuration["AzureFunction:Key"];

            //Datos que se envían a la function
            var payload = new
            {
                PresupuestoId = presupuestoId,
                UsuarioId = usuarioId,
                Email = email,
                MontoLimite = montoLimite,
                MontoActualGastado = montoActualGastado
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            //var requestUrl = urlFunction.Contains("?code=") ? urlFunction : $"{urlFunction}?code={keyFunction}";

            var response = await client.PostAsync(urlFunction, content);

            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error al llamar a la función de Azure. Status: {response.StatusCode}, Body: {body}");
            }

            // La Function devuelve JSON
            return body;
        }
    }

  
}
