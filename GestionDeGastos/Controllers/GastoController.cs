using GestionDeGastos.Models.Gasto;
using GestionDeGastos.Servicio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionDeGastos.Controllers
{
    public class GastoController : Controller
    {
        private readonly ICategoriaServicio _categoriaServicio;

        public GastoController(ICategoriaServicio categoriaServicio)
        {
            _categoriaServicio = categoriaServicio;
        }

        [HttpGet]
        public async Task<IActionResult> AgregarAsync()
        {

            List<Categorium> categoriasEntidad = (await _categoriaServicio.ObtenerTodasLasCategoriasDelUsuarioAsync(1)).ToList(); // valor hardcodeado, se tiene que sacar de la session 
            List<SelectListItem> categoriasSelect = categoriasEntidad.Select(c => new SelectListItem
            {
                Text = c.Descripcion,
                Value = c.IdCategoria.ToString()
            }).ToList();

            AgregarGastoViewModel gastoVMDefault = new AgregarGastoViewModel
            {
                OpcionTicketSeleccionada = "sin_ticket",
                Fecha = DateOnly.FromDateTime(DateTime.Now), // analizar si es necesario
                Categorias = categoriasSelect,
                //new List<SelectListItem>
                //{ // obtener de la db
                //    new SelectListItem { Text = "Alimentos", Value = "1" },
                //    new SelectListItem { Text = "Transporte", Value = "2" },
                //    new SelectListItem { Text = "Entretenimiento", Value = "3" }
                //},
                MetodosDePago = new List<SelectListItem>
                { // obtener de la db
                    new SelectListItem { Text = "Efectivo", Value = "1" },
                    new SelectListItem { Text = "Tarjeta de Credito", Value = "2" },
                    new SelectListItem { Text = "Tarjeta de Debito", Value = "3" },
                    new SelectListItem { Text = "Otro", Value = "4" }
                },
                Items = new List<AgregarGastoItemViewModel>() { new AgregarGastoItemViewModel() }
            };


            return View(gastoVMDefault);
        }

        [HttpPost]
        public IActionResult Agregar(AgregarGastoViewModel gastoVM)
        {

            return View(gastoVM);
        }
    }
}
