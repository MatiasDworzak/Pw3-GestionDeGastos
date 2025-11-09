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
<<<<<<< HEAD
      private readonly IHomeService _homeService;
      private readonly IUsuarioRepositorio _usuarioService;
      private readonly IUsuarioSession _usuarioSession;

        public HomeController(IHomeService homeService, IUsuarioRepositorio usuarioService,
           IUsuarioSession usuarioSession)
        {
         _homeService = homeService; 
         _usuarioService = usuarioService;
         _usuarioSession = usuarioSession;
=======
        private readonly IHomeService _homeService;
        private readonly IPresupuestoServicio _presupuestoService;
        private readonly IUsuarioRepositorio _usuarioService;

        public HomeController(IHomeService homeService, IUsuarioRepositorio usuarioService, IPresupuestoServicio presupuestoServicio
            )
        {
            _homeService = homeService;
            _presupuestoService = presupuestoServicio;
            _usuarioService = usuarioService;
>>>>>>> main
        }

        public async Task<IActionResult> Home()
        {
            var idUsuario = HttpContext.Session.GetInt32("UsuarioId");
            Presupuesto presupuesto = await _presupuestoService.ObtenerPresupuestoActualAsync(idUsuario.Value);

            await _presupuestoService.CalcularMontoActualGastado(idUsuario.Value, presupuesto);
            var usuarioEntidad = await _usuarioService.GetByIdAsync(idUsuario.Value);
            var usuarioViewModel = new UsuarioViewModel { IdUsuario = usuarioEntidad.IdUsuario, Nombre = usuarioEntidad.Nombre, Email = usuarioEntidad.Email };

            var lista = await _homeService.ObtenerUltimosCincoGastosPorIdDeUsuario(idUsuario.Value);
            var gastoModel = new GastoViewModel { Porcentaje = await _presupuestoService.ObtenerPresupuestoConPorcentaje(idUsuario.Value, presupuesto), ListaUltimosTresGastos = lista };

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

            // Agrupamos para calcular totales (para el top 3)
            var categoriasConTotales = query
                .GroupBy(g => new
                {
                    g.IdCategoriaNavigation.Descripcion,
                    g.IdCategoriaNavigation.Icono,
                    g.IdCategoriaNavigation.Color
                })
                .Select(g => new
                {
                    Categoria = g.Key.Descripcion,
                    Icono = g.Key.Icono,
                    Color = g.Key.Color,
                    Total = g.Sum(x => x.MontoTotal)
                })
                .OrderByDescending(x => x.Total)
                .ToList();

            // Obtenemos el top 3
            var top3 = categoriasConTotales.Take(3).ToList();

            // Ahora devolvemos los gastos individuales con su categoría y color
            var gastosDetallados = query
      .GroupBy(g => new
      {
<<<<<<< HEAD
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
=======
          g.IdCategoriaNavigation.Descripcion,
          g.IdCategoriaNavigation.Icono,
          g.IdCategoriaNavigation.Color
      })
      .Select(g => new
      {
          Categoria = g.Key.Descripcion,
          Icono = g.Key.Icono,
          Color = g.Key.Color,
          Gastos = g.Select(x => new
          {
              x.Nombre,
              x.MontoTotal,
              Fecha = x.Fecha
          }).ToList(),
          TotalCategoria = g.Sum(x => x.MontoTotal)
      })
      .OrderByDescending(g => g.TotalCategoria)
      .ToList();

            return Json(new { gastosDetallados, top3 });
        }

    }
>>>>>>> main
}
