using GestionDeGastos.Models;
using GestionDeGastos.Servicio.GastoEspecifico;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestionDeGastos.Controllers
{
    public class GastoEspecificoController : Controller
    {
        public IGastoEspecificoServicio _gastoEspecificoServicio;

        public GastoEspecificoController(IGastoEspecificoServicio gastoEspecificoServicio)
        {
            _gastoEspecificoServicio = gastoEspecificoServicio;
        }

        public async Task<IActionResult> GastoEspecifico(int id)
        {
            var gastoObtenido = await _gastoEspecificoServicio.ObtenerGastoPorID(id);
            if (gastoObtenido == null)
            {
                return RedirectToAction("Home", "Home");
            }

            var categoriaObtenida = await _gastoEspecificoServicio.ObtenerCategoriaDeUnGasto(gastoObtenido);
            var metodoDePagoObtenido = await _gastoEspecificoServicio.ObtenerMetodoDePagoDeUnGasto(gastoObtenido);
            var ticketObtenido = await _gastoEspecificoServicio.ObtenerTicketDeUnGasto(gastoObtenido);
            var itemsObtenidos = await _gastoEspecificoServicio.ObtenerItemsDeUnGasto(gastoObtenido);
            
            var model = new GastoEspecificoViewModel
            {
                Nombre = gastoObtenido.Nombre,
                Monto = gastoObtenido.MontoTotal,
                Categoria = new CategoriaGastoEspecificoViewModel
                {
                    IdCategoria = categoriaObtenida.IdCategoria,
                    NombreCategoria = categoriaObtenida.Descripcion
                },
                MetodoDePago = new MetodoDePagoGastoEspecificoViewModel
                {
                    IdMetodoDePago = metodoDePagoObtenido.IdMetodoPago,
                    NombreMetodoDePago = metodoDePagoObtenido.Descripcion
                },
                Fecha = gastoObtenido.Fecha,
                Ticket = new TicketGastoEspecificoViewModel
                {
                    IdTicket = ticketObtenido?.IdTicket ?? 0,
                    URLFoto = ticketObtenido?.RutaImagenBlob ?? string.Empty
                },
                Items = itemsObtenidos.Select(i => new ItemGastoEspecificoViewModel
                {
                    IdItem = i.IdItem,
                    Cantidad = i.Cantidad,
                    Descripcion = i.Descripcion,
                    PrecioUnitario = i.PrecioUnitario,
                    PrecioTotal = i.PrecioTotal
                }).ToList()
            };
            
            return View(model);
        }
    }
}
