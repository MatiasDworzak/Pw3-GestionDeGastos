using GestionDeGastos.Filtros;
using GestionDeGastos.Models;
using GestionDeGastos.Models.GastoModels;
using GestionDeGastos.Repositorio;
using GestionDeGastos.Servicio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace GestionDeGastos.Controllers
{

   [AutorizacionSession]
    public class HomeController : Controller
    {
      private readonly IHomeService _homeService;
      private readonly IUsuarioRepositorio _usuarioService;

        public HomeController(IHomeService homeService, IUsuarioRepositorio usuarioService)
        {
         _homeService = homeService; 
         _usuarioService = usuarioService;
        }

      public async Task<IActionResult> Home()
      {
         var idUsuario = HttpContext.Session.GetInt32("UsuarioId");
       

         var usuarioEntidad = await _usuarioService.GetByIdAsync(idUsuario.Value);
         var usuarioViewModel = new UsuarioViewModel { IdUsuario = usuarioEntidad.IdUsuario, Nombre = usuarioEntidad.Nombre, Email = usuarioEntidad.Email };

         var lista = await _homeService.ObtenerUltimosTresGastosPorIdDeUsuario(idUsuario.Value);
         var gastoModel = new GastoViewModel { Porcentaje = _homeService.ObtenerPresupuestoConPorcentaje(idUsuario.Value), ListaUltimosTresGastos = lista };

         ViewBag.UsuarioHeader = usuarioViewModel;
         return View(gastoModel);
      }
   }
}
