using GestionDeGastos.AccesoADatos.Entidades;
using GestionDeGastos.Models.Gasto;
using GestionDeGastos.Servicio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

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

            // 2. si es sin ticket se deberian ignorar los data anottations de la lista de items del view model y disparar las del gasto, y pedirle al servicio hacer un subir un gasto sin ticket (ignoramos la lista si es que viene con una)

            // 3. si es un ticket manual o ticket foto hay que disparar sus data anottations y tambien las de gasto, y se le deberia pedir al servicio realizar un item con ticket.
            // el con o sin foto es indiferente, ambos van a poseer ticket y la foto se renderizara o no en el front si es que viene en null o no. 

            // en el caso de que haya una falla se debe mostrar la vista de nuevo con lo que fallo y los datos que mando

            //bool esConTicket = gastoVM.OpcionTicketSeleccionada == "ticket_manual" ||
            //                   gastoVM.OpcionTicketSeleccionada == "ticket_foto";


            // 1. LÓGICA DE VALIDACIÓN CONDICIONAL
            if (gastoVM.OpcionTicketSeleccionada == "sin_ticket")
            {
                // Si no tiene ticket(ya sea manual o echo con foto, ignoramos las data anotattions de la foto y de los items)
                var itemKeys = ModelState.Keys.Where(k => k.StartsWith("Items") || k.StartsWith("TicketFoto")).ToList();
                foreach (var key in itemKeys) ModelState.Remove(key);
            }
            else
            {
                // Si tiene ticket ya sea por foto o manual, nos aseguramos de que el monto total
                // coincida con la suma.
                decimal montoCalculado = gastoVM.Items?.Sum(i => (i.Cantidad ?? 0) * (i.PrecioUnitario ?? 0)) ?? 0;
                if (montoCalculado != gastoVM.MontoTotal) ModelState.AddModelError(nameof(gastoVM.MontoTotal), "El monto total debe ser igual a la sumatoria entre los precios de los items por su cantidad");

                // Validación específica para "ticket_foto"
                //if (gastoVM.OpcionTicketSeleccionada == "ticket_foto")
                //{
                //    if (gastoVM.TicketFoto == null || gastoVM.TicketFoto.Length == 0)
                //    {
                //        ModelState.AddModelError(nameof(gastoVM.TicketFoto), "Debes adjuntar una foto para la opción 'Ticket con Foto'.");
                //    }
                //}

                if (gastoVM.OpcionTicketSeleccionada == "ticket_manual")
                {
                    // borramos solo las data anottations de la foto, porque en el manual no se necesita foto
                    var itemKeys = ModelState.Keys.Where(k => k.StartsWith("TicketFoto")).ToList();
                    foreach (var key in itemKeys) ModelState.Remove(key);
                }
            }
            // 2. COMPROBACIÓN FINAL DEL MODELO: en este punto estan las data anotattions moldeadas al tipo de alta que se decidio en el formulario
            if (ModelState.IsValid)
            {
                // ¡Validación exitosa!
                // Aquí va tu lógica para:
                // 1. Mapear 'gastoVM' a tus entidades de base de datos.
                // 2. Si es 'ticket_foto', subir 'gastoVM.TicketFoto' al Blob Storage y guardar la URL.
                // 3. Guardar el Gasto, el Ticket (si existe) y los Items (si existen) en la BD.

                // ej:
                // var nuevoGasto = new Gasto { ... };
                // await _gastoServicio.CrearGastoAsync(nuevoGasto, gastoVM.Items);

                return RedirectToAction("Index", "Home"); // O a donde quieras ir
            }

            // 3. SI EL MODELO NO ES VÁLIDO
            // Si llegamos aquí, algo falló. Volvemos a cargar la vista
            // con los mensajes de error.

            // ¡Importante! Debemos recargar los DropDownLists.
            


            return View(gastoVM);
        }
    }
}
