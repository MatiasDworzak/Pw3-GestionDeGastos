using GestionDeGastos.AccesoADatos.Entidades;
using GestionDeGastos.Models.Gasto;
using GestionDeGastos.Servicio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionDeGastos.Controllers
{
    public class GastoController : Controller
    {
        private readonly ICategoriaServicio _categoriaServicio;
        private readonly IMetodoDePagoServicio _metodoDePagoServicio;

        public GastoController(ICategoriaServicio categoriaServicio, IMetodoDePagoServicio metodoDePagoServicio)
        {
            _categoriaServicio = categoriaServicio;
            _metodoDePagoServicio = metodoDePagoServicio;
        }

        [HttpGet]
        public async Task<IActionResult> AgregarAsync()
        {

            List<Categorium> categoriasEntidad = (await _categoriaServicio.ObtenerTodasLasCategoriasDelUsuarioAsync(3)).ToList(); // valor hardcodeado, se tiene que sacar de la session 
            List<SelectListItem> categoriasSelect = categoriasEntidad.Select(c => new SelectListItem
            {
                Text = c.Descripcion,
                Value = c.IdCategoria.ToString()
            }).ToList();

            List<MetodoDePago> metodoDePagosEntidad = (await _metodoDePagoServicio.ObtenerTodosLosMetodosDePagoAsync()).ToList();
            List<SelectListItem> metodosDePagoSelect = metodoDePagosEntidad.Select(m => new SelectListItem
            {
                Text = m.Descripcion,
                Value = m.IdMetodoPago.ToString()
            }).ToList();

            AgregarGastoViewModel gastoVMDefault = new AgregarGastoViewModel
            {
                OpcionTicketSeleccionada = "sin_ticket",
                Fecha = DateOnly.FromDateTime(DateTime.Now), // analizar si es necesario
                Categorias = categoriasSelect,
                MetodosDePago = metodosDePagoSelect,
                Items = new List<AgregarGastoItemViewModel>() { new AgregarGastoItemViewModel() }
            };

            return View(gastoVMDefault);
        }

        [HttpPost]
        public IActionResult Agregar(AgregarGastoViewModel gastoVM)
        {
            // definir cuando evaluar el model state



            // 1. Determinar la opcion seleccionada de procesamiento de gasto (sin ticket, ticket manual o ticket foto)
            // antes de eso se deberia verificar que la parte obligatoria de gasto este

            // 2. si es sin ticket se deberian ignorar los data anottations de la lista de items del view model y disparar las del gasto, y pedirle al servicio hacer un subir un gasto sin ticket (ignoramos la lista si es que viene con una)

            // 3. si es un ticket manual o ticket foto hay que disparar sus data anottations y tambien las de gasto, y se le deberia pedir al servicio realizar un item con ticket.
            // el con o sin foto es indiferente, ambos van a poseer ticket y la foto se renderizara o no en el front si es que viene en null o no. 



            // en el caso de que haya una falla se debe mostrar la vista de nuevo con lo que fallo y los datos que mando
            return View(gastoVM);
        }
    }
}
