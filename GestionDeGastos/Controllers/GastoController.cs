using GestionDeGastos.Models.Gasto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionDeGastos.Controllers
{
    public class GastoController : Controller
    {
        [HttpGet]
        public IActionResult Agregar()
        {

            AgregarGastoViewModel viewModel = new AgregarGastoViewModel
            {
                OpcionTicketSeleccionada = "sin_ticket",
                Fecha = DateOnly.FromDateTime(DateTime.Now),
                Categorias = new List<SelectListItem>
                { // obtener de la db
                    new SelectListItem { Text = "Alimentos", Value = "1" },
                    new SelectListItem { Text = "Transporte", Value = "2" },
                    new SelectListItem { Text = "Entretenimiento", Value = "3" }
                },
                MetodosDePago = new List<SelectListItem>
                { // obtener de la db
                    new SelectListItem { Text = "Efectivo", Value = "1" },
                    new SelectListItem { Text = "Tarjeta de Credito", Value = "2" },
                    new SelectListItem { Text = "Tarjeta de Debito", Value = "3" },
                    new SelectListItem { Text = "Otro", Value = "4" }
                },
                Items = new List<AgregarGastoItemViewModel>() { new AgregarGastoItemViewModel() }
            };


            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Agregar(AgregarGastoViewModel gasto)
        {

            return View(gasto);
        }
    }
}
