using GestionDeGastos.AccesoADatos.Entidades;
using GestionDeGastos.Models;
using GestionDeGastos.Servicio;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeGastos.Controllers
{
   public class PresupuestoController : Controller
   {
      public IPresupuestoServicio _presupuestoServicio;
      public IVerTodosLosGastos _verTodosLosGastosServicio;

        public PresupuestoController(IPresupuestoServicio presupuestoServicio, IVerTodosLosGastos gastoServicio)
        {
            _presupuestoServicio = presupuestoServicio;
            _verTodosLosGastosServicio = gastoServicio;
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
        //    int idUsuario = ObtenerUsuarioLogueado();
        //    var detalleGastos = _verTodosLosGastosServicio.ObtenerLosGastosFiltradosPorMes(idUsuario, mes, anio);

        //    return View("VerTodosLosGastos");
        //}
    }
}