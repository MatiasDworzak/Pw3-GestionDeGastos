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

        public IPresupuestoServicio presupuestoServicio;

        public IActionResult PresupuestoActualBD()
        {
            var ultimoPresupuesto = presupuestoServicio.ObtenerPresupuestoActualAsync().Result;
            IEnumerable<Presupuesto> listaPresupuestos = presupuestoServicio.ObtenerTodosLosPresupuestosAsync().Result;

            var modelo = new PresupuestoPaginaViewModel
            {
                UltimoPresupuesto = new PresupuestoViewModel
                {
                    MontoLimite = ultimoPresupuesto.MontoLimite,
                    MontoActualGastado = ultimoPresupuesto.MontoActualGastado,
                },
                ListaPresupuestos = listaPresupuestos.Select(p => new PresupuestoViewModel
                {
                    MontoLimite = p.MontoLimite,
                    MontoActualGastado = p.MontoActualGastado
                }),
                PorcentajeGastado = presupuestoServicio.ObtenerPresupuestoConPorcentaje()
            };

            return View(modelo);
        }
    }
}
