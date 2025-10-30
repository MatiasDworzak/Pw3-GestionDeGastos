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
        public IActionResult Agregar()
        {

            AgregarGastoViewModel gastoVMDefault = new AgregarGastoViewModel
            {
                OpcionTicketSeleccionada = "sin_ticket",
                Fecha = DateOnly.FromDateTime(DateTime.Now),
                Items = new List<AgregarGastoItemViewModel>() { new AgregarGastoItemViewModel() }
            };

            CargarCategoriasYMediosDePago(gastoVMDefault);

            return View(gastoVMDefault);
        }

        [HttpPost]
        public IActionResult Agregar(AgregarGastoViewModel gastoVMRecibido)
        {
            ProcesarValidacionesPorTipoDeTicket(gastoVMRecibido);

            if (ModelState.IsValid)
            {
                // Aquí va tu lógica para:
                // 1. Mapear 'gastoVM' a tus entidades de base de datos.
                // 2. Si es 'ticket_foto', subir 'gastoVM.TicketFoto' al Blob Storage y guardar la URL.
                // 3. Guardar el Gasto, el Ticket (si existe) y los Items (si existen) en la BD.

                new Gasto()
                {
                    Nombre = gastoVMRecibido.Nombre,
                    Fecha = gastoVMRecibido.Fecha,
                    MontoTotal = gastoVMRecibido.MontoTotal.Value,
                    // inserto mediante id porque ya existen en la BD
                    IdUsuario = 3, // valor hardcodeado, se tiene que sacar de la session
                    IdMetodoPago = gastoVMRecibido.MetodoDePagoSeleccionado.Value, // fijarse si pasarlo a viewmodel
                    IdCategoria = gastoVMRecibido.CategoriaSeleccionada.Value, // fijarse si pasarlo a viewmodel
                    // inserto mediante el objeto Navigation asi se generan al mismo tiempo en la BD
                    IdTicketNavigation = new Ticket()
                    // falta terminar de mapear
                };

                // ej:
                // var nuevoGasto = new Gasto { ... };
                // await _gastoServicio.CrearGastoAsync(nuevoGasto, gastoVM.Items);

                return RedirectToAction("Index", "Home"); // O a donde quieras ir
            }

            CargarCategoriasYMediosDePago(gastoVMRecibido);

            return View(gastoVMRecibido);
        }

        // Metodos Helpers
        private void ProcesarValidacionesPorTipoDeTicket(AgregarGastoViewModel gastoVM)
        {
            if (string.IsNullOrEmpty(gastoVM.OpcionTicketSeleccionada))
            {
                ModelState.AddModelError(nameof(gastoVM.OpcionTicketSeleccionada), "Debe seleccionar una opción de ticket.");
                return;
            }

            switch (gastoVM.OpcionTicketSeleccionada)
            {
                case "sin_ticket":
                    BorrarDataAnnotationsDeAtributosDelModelState(["TicketFoto", "Items"]);
                    break;

                case "ticket_manual":
                    BorrarDataAnnotationsDeAtributosDelModelState(["TicketFoto"]);
                    ValidarMontoTotal(gastoVM);
                    break;

                case "ticket_foto":
                    ValidarMontoTotal(gastoVM);
                    break;

                default:
                    ModelState.AddModelError(nameof(gastoVM.OpcionTicketSeleccionada), "Opción de ticket no reconocida.");
                    break;
            }
        }

        private void ValidarMontoTotal(AgregarGastoViewModel gastoVM)
        {
            decimal montoCalculado = gastoVM.Items?.Sum(i => (i.Cantidad ?? 0) * (i.PrecioUnitario ?? 0)) ?? 0;

            if (montoCalculado != gastoVM.MontoTotal)
                ModelState.AddModelError(nameof(gastoVM.MontoTotal),
                    "El monto total debe ser igual a la sumatoria entre los precios de los items por su cantidad.");
        }

        private void CargarCategoriasYMediosDePago(AgregarGastoViewModel gastoVM)
        {
            // TODO: Analizar si despues hacer view models de Categoria y Metodo de pago por si se agregan colores e iconos, recordar usar for en el front para mostrarlos
            var categoriasEntidad = _categoriaServicio.ObtenerTodasLasCategoriasDelUsuarioAsync(3).Result; // valor hardcodeado, se tiene que sacar de la session 
            gastoVM.Categorias = categoriasEntidad.Select(c => new SelectListItem
            {
                Text = c.Descripcion,
                Value = c.IdCategoria.ToString()
            }).ToList();

            var metodosDePagoEntidad = _metodoDePagoServicio.ObtenerTodosLosMetodosDePagoAsync().Result;
            gastoVM.MetodosDePago = metodosDePagoEntidad.Select(m => new SelectListItem
            {
                Text = m.Descripcion,
                Value = m.IdMetodoPago.ToString()
            }).ToList();
        }

        private void BorrarDataAnnotationsDeAtributosDelModelState(string[] atributos)
        {
            foreach (var nombreDeAtributo in atributos)
            {
                var itemKeys = ModelState.Keys.Where(k => k.StartsWith(nombreDeAtributo)).ToList();
                foreach (var key in itemKeys) ModelState.Remove(key);
            }
        }
    }
}
