using GestionDeGastos.Filtros;
using GestionDeGastos.Models;
using Microsoft.AspNetCore.Mvc;
using GestionDeGastos.Servicio;
using System.Threading.Tasks;
using GestionDeGastos.Models.GastoModels;


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
<<<<<<< HEAD

         var model = new UsuarioViewModel
         {
            IdUsuario = HttpContext.Session.GetInt32("UsuarioId").Value,
            Nombre = HttpContext.Session.GetString("UsuarioNombre"),
            Email = HttpContext.Session.GetString("UsuarioEmail"),
         };
            return View(model);         
=======
            
            var lista = await _homeService.ObtenerUltimosTresGastosPorIdDeUsuario(1);
            var modelo = new GastoViewModel {Porcentaje = _homeService.ObtenerPresupuestoConPorcentaje(1), ListaUltimosTresGastos = lista};
           
            return  View(modelo);         
>>>>>>> MainTest
        }

       
    }
}
