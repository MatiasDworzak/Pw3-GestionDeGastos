using GestionDeGastos.Models;
using GestionDeGastos.Models.GastoModels;
using GestionDeGastos.Servicio;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestionDeGastos.Controllers
{
    public class VerTodosLosGastosController : Controller
    {


        private readonly IVerTodosLosGastos  _VerTodosLosGastosServicio;

        public VerTodosLosGastosController ( IVerTodosLosGastos verGastos)
        {
            _VerTodosLosGastosServicio = verGastos;
        }

        public async Task<IActionResult> VerTodosLosGastos()
        {
            int? idUsuarioLoguedo = HttpContext.Session.GetInt32("UsuarioId");
            int idUsuario = idUsuarioLoguedo.Value;

          var categoriaConMontoTotal = await _VerTodosLosGastosServicio.ObtenerTodosLosGastosFiltradosPorCategoria(idUsuario);
            var model = new GastoPorCategoriaViewModel
            {

                gasto = categoriaConMontoTotal

            };
          
            return View(model);
        }

        public IActionResult MostrarGastosPresupuesto(int mes, int anio)
        {
            int? idUsuarioLoguedo = HttpContext.Session.GetInt32("UsuarioId");
            int idUsuario = idUsuarioLoguedo.Value;
            var detalleGastos = _VerTodosLosGastosServicio.ObtenerLosGastosFiltradosPorMes(idUsuario, mes, anio);

            return View("VerTodosLosGastos");
        }
    }
}
