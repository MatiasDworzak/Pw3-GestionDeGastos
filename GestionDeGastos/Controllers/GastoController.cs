using GestionDeGastos.AccesoADatos.Entidades;
using GestionDeGastos.Models.Gasto;
using GestionDeGastos.Models.Gasto.Enums;
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
                OpcionTicketSeleccionada = TipoTicket.SinTicket,
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
                // 3. Guardar el Gasto, el Ticket (si existe) y los Items (si existen) en la BD.

                Gasto gastoEntidad = new Gasto()
                {
                    Nombre = gastoVMRecibido.Nombre,
                    Fecha = gastoVMRecibido.Fecha,
                    MontoTotal = gastoVMRecibido.MontoTotal.Value,
                    // inserto mediante id porque ya existen en la BD
                    IdUsuario = 3, // valor hardcodeado, se tiene que sacar de la session
                    IdMetodoPago = gastoVMRecibido.MetodoDePagoSeleccionado.Value, // fijarse si pasarlo a viewmodel
                    IdCategoria = gastoVMRecibido.CategoriaSeleccionada.Value, // fijarse si pasarlo a viewmodel
                    IdTicketNavigation = mapeoDeEntidadTicket(gastoVMRecibido)
                };

                try
                {
                    gastoEntidad.IdTicketNavigation.RutaImagenBlob = "ruta_falsa_para_probar"; // await _servicioBlob.SubirFotoAsync(gastoVMRecibido.TicketFoto);
                    //await _gastoServicio.AgregarGastoAsync(gastoEntidad, gastoVM.OpcionTicketSeleccionada);

                    TempData["GastoExitoso"] = "Se ha agregado el gasto con exito!";

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex) 
                {
                    TempData["ErrorEnSubida"] = ex.Message;
                }
            }

            CargarCategoriasYMediosDePago(gastoVMRecibido);

            return View(gastoVMRecibido);
        }

        // Metodos Helpers
        private void ProcesarValidacionesPorTipoDeTicket(AgregarGastoViewModel gastoVM)
        {
            if (gastoVM.OpcionTicketSeleccionada == null)
            {
                ModelState.AddModelError(nameof(gastoVM.OpcionTicketSeleccionada), "Debe seleccionar una opción de ticket.");
                return;
            }

            switch (gastoVM.OpcionTicketSeleccionada)
            {
                case TipoTicket.SinTicket:
                    BorrarDataAnnotationsDeAtributosDelModelState(["TicketFoto", "Items"]);
                    break;

                case TipoTicket.TicketManual:
                    BorrarDataAnnotationsDeAtributosDelModelState(["TicketFoto"]);
                    ValidarMontoTotal(gastoVM);
                    break;

                case TipoTicket.TicketFoto:
                    ValidarMontoTotal(gastoVM);
                    break;

                default:
                    ModelState.AddModelError(nameof(gastoVM.OpcionTicketSeleccionada), "Opción de ticket no reconocida.");
                    break;
            }
        }

        private void BorrarDataAnnotationsDeAtributosDelModelState(string[] atributos)
        {
            foreach (var nombreDeAtributo in atributos)
            {
                var itemKeys = ModelState.Keys.Where(k => k.StartsWith(nombreDeAtributo)).ToList();
                foreach (var key in itemKeys) ModelState.Remove(key);
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

        private Ticket mapeoDeEntidadTicket(AgregarGastoViewModel gastoVM)
        {
            if (gastoVM.OpcionTicketSeleccionada == TipoTicket.SinTicket 
                || gastoVM.Items == null 
                || gastoVM.Items.Count == 0) return null;
            
            return new Ticket
            {
                Items = gastoVM.Items?.Select(i => new Item
                {
                    Descripcion = i.Descripcion,
                    Cantidad = i.Cantidad.Value,
                    PrecioUnitario = i.PrecioUnitario.Value,
                    PrecioTotal = i.Cantidad.Value * i.PrecioUnitario.Value
                }).ToList()
            };
        }
    }
}
