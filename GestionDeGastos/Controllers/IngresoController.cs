using GestionDeGastos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeGastos.Controllers
{
   public class IngresoController : Controller
   {
      public ActionResult Login()
      {
         return View();
      }


      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Login(CredencialViewModel model)
      {
         if (!ModelState.IsValid) {
            return View(model);
         }

         TempData["mensajeEXito"] = "Sesion iniciada con éxito";
         return RedirectToAction("Login");
      }

     
   }
}
