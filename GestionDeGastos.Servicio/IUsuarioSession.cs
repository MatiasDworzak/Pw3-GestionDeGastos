using System.Text;
using Microsoft.AspNetCore.Http;

namespace GestionDeGastos.Servicio
{
   public interface IUsuarioSession
   {
      int? ObtenerUsuarioId();
      string? ObtenerNombre();
      string? ObtenerEmail();
      bool IsLoggedIn();

   }

   public class UsuarioSession : IUsuarioSession
   {
      private readonly IHttpContextAccessor _httpContextAccessor;

     public UsuarioSession(IHttpContextAccessor httpContextAccessor)
      {
         _httpContextAccessor = httpContextAccessor;
      }

      private ISession Session => _httpContextAccessor.HttpContext.Session;
      public bool IsLoggedIn()
         => ObtenerUsuarioId() != null;

      public string? ObtenerEmail()
         => Session.GetString("UsuarioEmail");

      public string? ObtenerNombre()
         =>Session.GetString("UsuarioNombre");

      public int? ObtenerUsuarioId()
         => Session.GetInt32("UsuarioId");
     
   }
}
