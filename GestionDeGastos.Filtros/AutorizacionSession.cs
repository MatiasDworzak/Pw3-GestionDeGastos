using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GestionDeGastos.Filtros
{
   public class AutorizacionSession : ActionFilterAttribute
   {
      public override void OnActionExecuting(ActionExecutingContext context)
      {
         var userIdSession = context.HttpContext.Session.GetString("UsuarioId");
         if (string.IsNullOrEmpty(userIdSession))
         {
            context.Result = new RedirectToActionResult("Login", "Ingreso", null);
         }
         base.OnActionExecuting(context);
      }
   }
}
