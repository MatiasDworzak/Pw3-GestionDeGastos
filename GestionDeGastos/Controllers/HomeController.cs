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
      private readonly IUsuarioSession _usuarioSession;

        public HomeController(IHomeService homeService, IUsuarioRepositorio usuarioService,
           IUsuarioSession usuarioSession)
        {
         _homeService = homeService; 
         _usuarioService = usuarioService;
         _usuarioSession = usuarioSession;
        }

      public async Task<IActionResult> Home()
      {
         var model = new UsuarioViewModel
         {
            IdUsuario = (int)_usuarioSession.ObtenerUsuarioId(),
            Nombre = _usuarioSession.ObtenerNombre(),
            Email = _usuarioSession.ObtenerEmail()
         };

         var lista = await _homeService.ObtenerUltimosTresGastosPorIdDeUsuario(model.IdUsuario);
         var gastoModel = new GastoViewModel { Porcentaje = _homeService.ObtenerPresupuestoConPorcentaje(model.IdUsuario), ListaUltimosTresGastos = lista };

         ViewBag.UsuarioHeader = model;
         return View(gastoModel);
      }
   }
}
