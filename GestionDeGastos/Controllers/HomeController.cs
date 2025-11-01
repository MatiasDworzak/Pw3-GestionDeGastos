using GestionDeGastos.AccesoADatos.Entidades;
using GestionDeGastos.Filtros;
using GestionDeGastos.Models;
using GestionDeGastos.Models.GastoModels;
using GestionDeGastos.Repositorio;
using GestionDeGastos.Servicio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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

        [HttpGet]
        public async Task<IActionResult> Filtrar(string mes, DateOnly? desde, DateOnly? hasta)
        {
            var idUsuario = HttpContext.Session.GetInt32("UsuarioId");
            if (idUsuario == null)
                return Unauthorized();

            List<Gasto> query = new List<Gasto>();

            if (!string.IsNullOrEmpty(mes))
            {
                var partes = mes.Split('-');
                int anio = int.Parse(partes[0]);
                int mesNumero = int.Parse(partes[1]);
                query = await _homeService.ObtenerLosGastosFiltradosPorMes(idUsuario, mesNumero, anio);
            
            }
            else if (desde.HasValue && hasta.HasValue)
            {

                 query = await _homeService.ObtenerGastosPorRangoDeFechasAsync(idUsuario, desde, hasta);
                

            }

            var resultado =  query.GroupBy(g => g.IdCategoriaNavigation.Descripcion)
                .Select(g => new
                {
                    Categoria = g.Key,
                    Monto = g.Sum(x => x.MontoTotal)
                })
                .ToList();

            return Json(resultado);
        }
    }
}
