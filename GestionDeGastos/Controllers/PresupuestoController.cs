using GestionDeGastos.AccesoADatos.Entidades;
using GestionDeGastos.Models;
using GestionDeGastos.Servicio;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestionDeGastos.Controllers
{
    public class PresupuestoController : Controller
    {
        public readonly IPresupuestoServicio _presupuestoServicio;
        public readonly IVerTodosLosGastos _verTodosLosGastosServicio;
        public readonly ILimiteDePresupuestoServicio _limiteDePresupuestoServicio;

        public PresupuestoController(IPresupuestoServicio presupuestoServicio,
                                    IVerTodosLosGastos gastoServicio,
                                    ILimiteDePresupuestoServicio limiteDePresupuestoServicio)
        {
            _presupuestoServicio = presupuestoServicio;
            _verTodosLosGastosServicio = gastoServicio;
            _limiteDePresupuestoServicio = limiteDePresupuestoServicio;
        }

        public async Task<IActionResult> PresupuestoActual()
        {
            int idUsuario = ObtenerUsuarioLogueado();

            var ultimoPresupuesto = await _presupuestoServicio.ObtenerPresupuestoActualAsync(idUsuario);

            await _presupuestoServicio.CrearPresupuesto(idUsuario, ultimoPresupuesto);

            IEnumerable<Presupuesto> listaPresupuestos = await _presupuestoServicio.ObtenerTodosLosPresupuestosAsync(idUsuario);

            PresupuestoPaginaViewModel modelo = await ContenidoPresupuestoViewModel(ultimoPresupuesto, listaPresupuestos);
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> PresupuestoActual(Presupuesto presupuesto, decimal NuevoMonto)
        {
            int idUsuario = ObtenerUsuarioLogueado();

            Presupuesto ultimoPresupuesto = await _presupuestoServicio.ObtenerPresupuestoActualAsync(idUsuario);
            IEnumerable<Presupuesto> listaPresupuestos = await _presupuestoServicio.ObtenerTodosLosPresupuestosAsync(idUsuario);

            await _presupuestoServicio.ActualizarPresupuestoAsync(idUsuario, ultimoPresupuesto, NuevoMonto);

            PresupuestoPaginaViewModel modelo = await ContenidoPresupuestoViewModel(ultimoPresupuesto, listaPresupuestos);
            return View(modelo);
        }

        private int ObtenerUsuarioLogueado()
        {
            int? idUsuarioLoguedo = HttpContext.Session.GetInt32("UsuarioId");
            int idUsuario = idUsuarioLoguedo.Value;
            return idUsuario;
        }

        //private async Task<PresupuestoPaginaViewModel> ContenidoPresupuestoViewModel(Presupuesto ultimoPresupuesto, IEnumerable<Presupuesto> listaPresupuestos)
        //{

        //    int idUsuario = ObtenerUsuarioLogueado();
        //    await _presupuestoServicio.CalcularMontoActualGastado(idUsuario, ultimoPresupuesto);
        //    decimal porcentaje = await _presupuestoServicio.ObtenerPresupuestoConPorcentaje(idUsuario, ultimoPresupuesto);

        //    return new PresupuestoPaginaViewModel
        //    {
        //        UltimoPresupuesto = new PresupuestoViewModel
        //        {
        //            MontoLimite = ultimoPresupuesto.MontoLimite,
        //            MontoActualGastado = ultimoPresupuesto.MontoActualGastado
        //        },
        //        ListaPresupuestos = (List<PresupuestoViewModel>)listaPresupuestos.Select(p => new PresupuestoViewModel
        //        {
        //            MontoLimite = p.MontoLimite,
        //            MontoActualGastado = p.MontoActualGastado,
        //            Anio = p.Anio,
        //            Mes = p.Mes
        //        }).ToList(),
        //        PorcentajeGastado = porcentaje
        //    };
        //}

        private async Task<PresupuestoPaginaViewModel> ContenidoPresupuestoViewModel(
                                                        Presupuesto ultimoPresupuesto,
                                                        IEnumerable<Presupuesto> listaPresupuestos)
        {
            int idUsuario = ObtenerUsuarioLogueado();

            await _presupuestoServicio.CalcularMontoActualGastado(idUsuario, ultimoPresupuesto);
            decimal porcentaje = await _presupuestoServicio.ObtenerPresupuestoConPorcentaje(idUsuario, ultimoPresupuesto);

            string userEmail = HttpContext.Session.GetString("UsuarioEmail");
            //llamar a la Azure Function para enviar alerta si corresponde
            try
            {
                if (ultimoPresupuesto.MontoLimite.HasValue && ultimoPresupuesto.MontoActualGastado.HasValue)
                {
                    await _limiteDePresupuestoServicio.EnviarAlertaSiCorrespondeAsync(
                        ultimoPresupuesto.IdPresupuesto,
                        idUsuario,
                        userEmail,
                        ultimoPresupuesto.MontoLimite.Value,
                        ultimoPresupuesto.MontoActualGastado.Value
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Azure Function: {ex.Message}");
            }

            return new PresupuestoPaginaViewModel
            {
                UltimoPresupuesto = new PresupuestoViewModel
                {
                    MontoLimite = ultimoPresupuesto.MontoLimite,
                    MontoActualGastado = ultimoPresupuesto.MontoActualGastado
                },
                ListaPresupuestos = (List<PresupuestoViewModel>)listaPresupuestos.Select(p => new PresupuestoViewModel
                {
                    MontoLimite = p.MontoLimite,
                    MontoActualGastado = p.MontoActualGastado,
                    Anio = p.Anio,
                    Mes = p.Mes
                }).ToList(),
                PorcentajeGastado = porcentaje
            };
        }

        //public IActionResult MostrarGastosPresupuesto(int mes, int anio)
        //{
        //    int idUsuario = ObtenerUsuarioLogueado();
        //    var detalleGastos = _verTodosLosGastosServicio.ObtenerGastosPorMesAsync(idUsuario, mes, anio);

        //    return View("VerTodosLosGastos");
        //}
    }
}