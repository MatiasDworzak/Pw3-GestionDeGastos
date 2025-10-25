using System.Diagnostics;
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
         
            return View();
        }

       
    }
}
