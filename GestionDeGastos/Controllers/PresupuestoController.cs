using GestionDeGastos.AccesoADatos.Entidades;
using GestionDeGastos.Models;
using GestionDeGastos.Servicio;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeGastos.Controllers
{
   public class PresupuestoController : Controller
   {
      //public IActionResult PresupuestoActual()
      //{
      //    var listaPresupuestos = new List<PresupuestoViewModel>
      //    {
      //        new PresupuestoViewModel
      //        {
      //            MontoLimite = 1500.00m,
      //            MontoActualGastado = 750.00m,
      //            Anio = 2025,
      //            Mes = 05
      //        },
      //        new PresupuestoViewModel
      //        {
      //            MontoLimite = 2000.00m,
      //            MontoActualGastado = 1200.00m,
      //            Anio = 2025,
      //            Mes = 06
      //        }
      //    };

      //    var modelo = new PresupuestoPaginaViewModel
      //    {
      //        ListaPresupuestos = listaPresupuestos,
      //        UltimoPresupuesto = listaPresupuestos.LastOrDefault()
      //    };

      //    return View(modelo);
      //}

      public IPresupuestoServicio _presupuestoServicio;

      public PresupuestoController(IPresupuestoServicio presupuestoServicio)
      {
         _presupuestoServicio = presupuestoServicio;
      }

      public async Task<IActionResult> PresupuestoActual()
      { 
         int idUsuario = ObtenerUsuarioLogueado();

         var ultimoPresupuesto = await _presupuestoServicio.ObtenerPresupuestoActualAsync(idUsuario);
         IEnumerable<Presupuesto> listaPresupuestos = await _presupuestoServicio.ObtenerTodosLosPresupuestosAsync(idUsuario);

         PresupuestoPaginaViewModel modelo = ContenidoPresupuestoViewModel(ultimoPresupuesto, listaPresupuestos);

         return View(modelo);
      }

      [HttpPost]
      public async Task<IActionResult> PresupuestoActual(Presupuesto presupuesto, decimal NuevoMonto)
      {
         int idUsuario = ObtenerUsuarioLogueado();

         Presupuesto ultimoPresupuesto = await _presupuestoServicio.ObtenerPresupuestoActualAsync(idUsuario);
         IEnumerable<Presupuesto> listaPresupuestos = await _presupuestoServicio.ObtenerTodosLosPresupuestosAsync(idUsuario);

         await _presupuestoServicio.ActualizarPresupuestoAsync(ultimoPresupuesto, NuevoMonto);

         PresupuestoPaginaViewModel modelo = ContenidoPresupuestoViewModel(ultimoPresupuesto, listaPresupuestos);
         return View(modelo);
      }

      private int ObtenerUsuarioLogueado()
      {
         //HttpContext.Session.SetInt32("UsuarioId", 1);
         // Para prueba de session

         int? idUsuarioLoguedo = HttpContext.Session.GetInt32("UsuarioId");
         int idUsuario = idUsuarioLoguedo.Value;
         return idUsuario;
      }

      private PresupuestoPaginaViewModel ContenidoPresupuestoViewModel(Presupuesto ultimoPresupuesto, IEnumerable<Presupuesto> listaPresupuestos)
      {

         int idUsuario = ObtenerUsuarioLogueado();

         return new PresupuestoPaginaViewModel
         {
            UltimoPresupuesto = new PresupuestoViewModel
            {
               MontoLimite = ultimoPresupuesto.MontoLimite,
               MontoActualGastado = ultimoPresupuesto.MontoActualGastado
            },
            ListaPresupuestos = (List<PresupuestoViewModel>)listaPresupuestos.Select(p => new PresupuestoViewModel
            {
               MontoLimite = p.MontoLimite,
               MontoActualGastado = p.MontoActualGastado,
               Anio = p.Anio,
               Mes = p.Mes
            }).ToList(),
            PorcentajeGastado = _presupuestoServicio.ObtenerPresupuestoConPorcentaje(idUsuario)
         };
      }

      //public IActionResult MostrarGastosPresupuesto(int mes, int anio)
      //{
      //    var detalleGastos = _gastoServicio.ObtenerGastosPorMesYAnio(mes, anio);

      //    return View("VerTodos");
      //}
   }
}