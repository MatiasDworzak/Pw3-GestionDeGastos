using GestionDeGastos.Filtros;
using GestionDeGastos.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeGastos.Controllers
{

   [AutorizacionSession]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Inicio()
        {

         var model = new UsuarioViewModel
         {
            IdUsuario = HttpContext.Session.GetInt32("UsuarioId").Value,
            Nombre = HttpContext.Session.GetString("UsuarioNombre"),
            Email = HttpContext.Session.GetString("UsuarioEmail"),
         };
            return View(model);         
        }

       
    }
}
