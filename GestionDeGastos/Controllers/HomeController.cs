using System.Diagnostics;
using GestionDeGastos.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeGastos.Controllers
{
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
