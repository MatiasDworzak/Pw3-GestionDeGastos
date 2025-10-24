using AutoMapper;
using GestionDeGastos.Models;
using GestionDeGastos.Servicio;
using GestionDeGastos.Servicio.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeGastos.Controllers
{
   public class IngresoController : Controller
   {
      private readonly IAutenticacionServicio _autenticacionServicio;
     
      public IngresoController(IAutenticacionServicio autenticacion) 
      {
         _autenticacionServicio = autenticacion;
      }
      public ActionResult Register()
      {
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Register(RegistroViewModel model)
      {
         if (!ModelState.IsValid) 
         {
            return View(model);
         }
         var usuario = new Usuario
         {
            Nombre = model.Nombre,
            Email = model.Correo,
            Contrasenia = model.Contrasenia
         };
         var usuarioRegistrado =   await _autenticacionServicio.RegistrarUsuarioAsync(usuario);

         if (usuarioRegistrado == null) {
            ModelState.AddModelError(model.Correo, "El correo electrónico ya está registrado.");
            return View(model);

         }
         Console.WriteLine("MI ID",usuarioRegistrado.IdUsuario);
         

         TempData["RegistroExito"] = $"Hola! {model.Nombre}, registrado con éxito\nIniciá sesión";
         return View("Login");
      }
      public ActionResult Login()
      {
         return View();
      }
      
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Login(CredencialViewModel model)
      {
         if (!ModelState.IsValid) {
            return View(model);
         }
         var usuarioValidado = await _autenticacionServicio.ValidarCredencialesAsync(model.Correo,model.Contrasenia);
         if(usuarioValidado == null)
         {
            ModelState.AddModelError(string.Empty, "Correo o contraseña inválidos."); 
            return View(model);
         }

         HttpContext.Session.SetInt32("UsuarioId", usuarioValidado.IdUsuario);
         HttpContext.Session.SetString("UsuarioNombre", usuarioValidado.Nombre);
         HttpContext.Session.SetString("UsuarioEmail", usuarioValidado.Email);

         TempData["LoginExito"] = "Sesion iniciada con éxito";
         return RedirectToAction("Inicio", "Home");
      }
      [HttpPost]
      [ValidateAntiForgeryToken]
      public IActionResult Logout()
      {
         HttpContext.Session.Remove("UsuarioId");
         HttpContext.Session.Remove("UsuarioNombre");
         HttpContext.Session.Remove("UsuarioEmail");

         return RedirectToAction("Login"); 
      }

     

   }
}
