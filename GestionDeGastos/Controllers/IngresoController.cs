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

         TempData["LoginExito"] = "Sesion iniciada con éxito";
         return RedirectToAction("Inicio", "Home");
      }


      public ActionResult Register()
      {
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Register(RegistroViewModel model)
      {
         if (!ModelState.IsValid) 
         {
            return View(model);
         }

         TempData["RegistroExito"] = $"Hola! {model.Nombre}, registrado con éxito\nIniciá sesión";
         return View("Login");
      }

   }
}
