using GestionDeGastos.Models;
using GestionDeGastos.Servicio.GastoEspecifico;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeGastos.Controllers
{
    public class GastoEspecificoController : Controller
    {
        public IGastoEspecificoServicio _gastoEspecificoServicio;

        public GastoEspecificoController(IGastoEspecificoServicio gastoEspecificoServicio)
        {
            _gastoEspecificoServicio = gastoEspecificoServicio;
        }

        public IActionResult GastoEspecifico(int id)
        {
            var gastoObtenido = _gastoEspecificoServicio.ObtenerGastoPorID(id);
            var model = new GastoEspecificoViewModel
            {
                Nombre = gastoObtenido.Result.Nombre,
                Monto = gastoObtenido.Result.MontoTotal,
                //Categoria = gastoObtenido.Result.IdCategoriaNavigation.ToString(),
                //MetodoDePago = gastoObtenido.Result.IdMetodoPagoNavigation.ToString(),
                Fecha = gastoObtenido.Result.Fecha,
                //URLFoto = gastoObtenido.Result.IdCategoriaNavigation.ToString(),
                //Items = gastoObtenido.Result.Item.Select(i => new ItemGastoEspecificoViewModel
                //{
                //    Cantidad = i.Cantidad,
                //    Descripcion = i.Descripcion,
                //    PrecioUnitario = i.PrecioUnitario,
                //    PrecioTotal = i.PrecioTotal
                //}).ToList()
            };
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        //public IActionResult GastoEspecifico()
        //{
        //    var model = new GastoEspecificoViewModel
        //    {
        //        Nombre = "Compra de supermercado",
        //        Monto = 150,
        //        Categoria = "Alimentos",
        //        MetodoDePago = "Tarjeta de crédito",
        //        Fecha = DateTime.Now,
        //        URLFoto = "/images/recibo.jpg",
        //        Items = new List<ItemGastoEspecificoViewModel>
        //        {
        //            new ItemGastoEspecificoViewModel
        //            {
        //                Cantidad = 2,
        //                Descripcion = "Manzanas",
        //                PrecioUnitario = 3.5m,
        //                PrecioTotal = 7.0m
        //            },
        //            new ItemGastoEspecificoViewModel
        //            {
        //                Cantidad = 1,
        //                Descripcion = "Pan",
        //                PrecioUnitario = 2.0m,
        //                PrecioTotal = 2.0m
        //            },
        //            new ItemGastoEspecificoViewModel
        //            {
        //                Cantidad = 5,
        //                Descripcion = "Leche",
        //                PrecioUnitario = 4.0m,
        //                PrecioTotal = 20.0m
        //            }
        //        }
        //    };
        //    return View(model);
        //}
    }
}
