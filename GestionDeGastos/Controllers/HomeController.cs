using System.Diagnostics;
using GestionDeGastos.Filtros;
using GestionDeGastos.Models;
using Microsoft.AspNetCore.Mvc;
using GestionDeGastos.Servicio;
using System.Threading.Tasks;


namespace GestionDeGastos.Controllers
{

   [AutorizacionSession]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService _homeService;

        public HomeController(ILogger<HomeController> logger, IHomeService homeService)
        {
            _logger = logger;
            _homeService = homeService; 
        }

        public async Task<IActionResult> Home()
        {
            var p = await _homeService.ObtenerPresupuestoPorIdAsync(1);
            var modelo = new PresupuestoViewModel { MontoActualGastado = p.MontoActualGastado,
                MontoLimite = p.MontoLimite};
           
            return View(modelo);         
        }

       
    }
}
