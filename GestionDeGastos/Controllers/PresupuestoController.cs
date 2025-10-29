using GestionDeGastos.Models;
using GestionDeGastos.Servicios;
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
             HttpContext.Session.SetInt32("UsuarioId", 1);
            // prueba de session

            int? idUsuarioLoguedo = HttpContext.Session.GetInt32("UsuarioId");
            if (!idUsuarioLoguedo.HasValue)
            {
                return RedirectToAction("Login");
            }

            int idUsuario = idUsuarioLoguedo.Value;

            var ultimoPresupuesto = await _presupuestoServicio.ObtenerPresupuestoActualAsync(idUsuario);
            IEnumerable<Presupuesto> listaPresupuestos = await _presupuestoServicio.ObtenerTodosLosPresupuestosAsync(idUsuario);

            PresupuestoPaginaViewModel modelo = ContenidoPresupuestoViewModel(ultimoPresupuesto, listaPresupuestos);

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> PresupuestoActual(Presupuesto presupuesto, decimal NuevoMonto)
        {

            Presupuesto ultimoPresupuesto = await _presupuestoServicio.ObtenerPresupuestoActualAsync(1);
            IEnumerable<Presupuesto> listaPresupuestos = await _presupuestoServicio.ObtenerTodosLosPresupuestosAsync(1);

            await _presupuestoServicio.ActualizarPresupuestoAsync(ultimoPresupuesto, NuevoMonto);

            PresupuestoPaginaViewModel modelo = ContenidoPresupuestoViewModel(ultimoPresupuesto, listaPresupuestos);
            return View(modelo);
        }

        private PresupuestoPaginaViewModel ContenidoPresupuestoViewModel(Presupuesto ultimoPresupuesto, IEnumerable<Presupuesto> listaPresupuestos)
        {
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
                PorcentajeGastado = _presupuestoServicio.ObtenerPresupuestoConPorcentaje(1)
            };
        }
    }
}
