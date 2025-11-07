using GestionDeGastos.AccesoADatos.Entidades;
using GestionDeGastos.Models.GastoModels;

using GestionDeGastos.Servicio;
using GestionDeGastos.Servicio.Enums;
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
        private readonly IGastoServicio _gastoServicio;
        private readonly IBlobAzureServicio _servicioBlob;

        public GastoController(ICategoriaServicio categoriaServicio, IMetodoDePagoServicio metodoDePagoServicio, IGastoServicio gastoServicio, IBlobAzureServicio servicioBlob)
        {
            _categoriaServicio = categoriaServicio;
            _metodoDePagoServicio = metodoDePagoServicio;
            _gastoServicio = gastoServicio;
            _servicioBlob = servicioBlob;
        }

        [HttpGet]
        public async Task<IActionResult> Agregar()
        {

            AgregarGastoViewModel gastoVMDefault = new AgregarGastoViewModel
            {
                OpcionTicketSeleccionada = TipoTicket.SinTicket,
                Fecha = DateOnly.FromDateTime(DateTime.Now),
                Items = new List<AgregarGastoItemViewModel>() { new AgregarGastoItemViewModel() }
            };

            await CargarCategoriasYMediosDePago(gastoVMDefault);

            return View(gastoVMDefault);
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(AgregarGastoViewModel gastoVMRecibido)
        {
            ProcesarValidacionesPorTipoDeTicket(gastoVMRecibido);

            if (ModelState.IsValid)
            {
                // Guardar el Gasto, el Ticket (si existe) y los Items (si existen) en la BD.

                Gasto gastoEntidad = new Gasto()
                {
                    Nombre = gastoVMRecibido.Nombre,
                    Fecha = gastoVMRecibido.Fecha,
                    MontoTotal = gastoVMRecibido.MontoTotal.Value,
                    // inserto mediante id porque ya existen en la BD
                    IdUsuario = ObtenerUsuarioLogueado(),
                    IdMetodoPago = gastoVMRecibido.MetodoDePagoSeleccionado.Value, // fijarse si pasarlo a viewmodel
                    IdCategoria = gastoVMRecibido.CategoriaSeleccionada.Value, // fijarse si pasarlo a viewmodel
                    IdTicketNavigation = mapeoDeEntidadTicket(gastoVMRecibido)
                };

                try
                {
                    
                    if (gastoEntidad.IdTicketNavigation != null
                        && gastoVMRecibido.OpcionTicketSeleccionada == TipoTicket.TicketFoto)
                        gastoEntidad.IdTicketNavigation.RutaImagenBlob = await _servicioBlob.SubirBlobAsync(gastoVMRecibido.TicketFoto, "tickets");

                    await _gastoServicio.AgregarGastoAsync(gastoEntidad);

                    TempData["GastoExitoso"] = "Se ha agregado el gasto con exito!";

                    return RedirectToAction("Home", "Home");
                }
                catch (Exception ex) 
                {
                    if(gastoEntidad.IdTicketNavigation?.RutaImagenBlob != null) 
                        _servicioBlob.EliminarBlob(gastoEntidad.IdTicketNavigation.RutaImagenBlob, "tickets");

                    TempData["ErrorEnSubida"] = ex.Message;
                }
            }
                
            await CargarCategoriasYMediosDePago(gastoVMRecibido);

            return View(gastoVMRecibido);
        }

        // Metodos Helpers

        private int ObtenerUsuarioLogueado()
        {
            int? idUsuarioLoguedo = HttpContext.Session.GetInt32("UsuarioId");
            int idUsuario = idUsuarioLoguedo.Value;
            return idUsuario;
        }
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

        private async Task CargarCategoriasYMediosDePago(AgregarGastoViewModel gastoVM)
        {
            // TODO: Analizar si despues hacer view models de Categoria y Metodo de pago por si se agregan colores e iconos, recordar usar for en el front para mostrarlos
            var categoriasEntidad = await _categoriaServicio.ObtenerTodasLasCategoriasDelUsuarioAsync(ObtenerUsuarioLogueado()); // valor hardcodeado, se tiene que sacar de la session 
            gastoVM.Categorias = categoriasEntidad.Select(c => new SelectListItem
            {
                Text = c.Descripcion,
                Value = c.IdCategoria.ToString()
            }).ToList();

            var metodosDePagoEntidad = await _metodoDePagoServicio.ObtenerTodosLosMetodosDePagoAsync();
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
                Items = gastoVM.Items.Select(i => new Item
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
