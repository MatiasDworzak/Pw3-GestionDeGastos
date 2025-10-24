using GestionDeGastos.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeGastos.Controllers
{
    public class PresupuestoController : Controller
    {
        //Usar esta funcion cuando tenga registros en la base de datos
        //public IActionResult PresupuestoActual(PresupuestoViewModel model)
        //{
        //    return View(model);
        //}

        public IActionResult PresupuestoActual()
        {
            var listaPresupuestos = new List<PresupuestoViewModel>
            {
                new PresupuestoViewModel
                {
                    MontoLimite = 1500.00m,
                    MontoActualGastado = 750.00m,
                    Anio = 2025,
                    Mes = 05
                },
                new PresupuestoViewModel
                {
                    MontoLimite = 2000.00m,
                    MontoActualGastado = 1200.00m,
                    Anio = 2025,
                    Mes = 06
                }
            };

            var modelo = new PresupuestoPaginaViewModel
            {
                ListaPresupuestos = listaPresupuestos,
                UltimoPresupuesto = listaPresupuestos.LastOrDefault()
            };

            return View(modelo);
        }
    }
}
