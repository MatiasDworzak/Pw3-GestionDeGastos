using GestionDeGastos.Models.GastoModels;
using GestionDeGastos.Servicio;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GestionDeGastos.Controllers
{
    public class VerTodosLosGastosController : Controller
    {
        private readonly IVerTodosLosGastos _servicio;

        public VerTodosLosGastosController(IVerTodosLosGastos servicio)
        {
            _servicio = servicio;
        }

        // Vista principal
        public async Task<IActionResult> VerTodosLosGastos()
        {
            int? idUsuarioLogueado = HttpContext.Session.GetInt32("UsuarioId");
            if (idUsuarioLogueado == null) return RedirectToAction("Login", "Usuario");

            int idUsuario = idUsuarioLogueado.Value;

            var model = new GastoPorCategoriaViewModel
            {
                UltimosGastos = await _servicio.ObtenerTodosLosGastos(idUsuario),
                TotalesPorCategoria = await _servicio.ObtenerTodosLosGastosFiltradosPorCategoria(idUsuario)
            };

            return View(model);
        }

        // Filtro por mes
        [HttpGet]
        public async Task<IActionResult> FiltrarPorMes(int mes, int año)
        {
            int? idUsuarioLogueado = HttpContext.Session.GetInt32("UsuarioId");
            if (idUsuarioLogueado == null) return RedirectToAction("Login", "Usuario");

            int idUsuario = idUsuarioLogueado.Value;

            var model = new GastoPorCategoriaViewModel
            {
                UltimosGastos = await _servicio.ObtenerGastosPorMesAsync(idUsuario, mes, año),
                TotalesPorCategoria = await _servicio.ObtenerTodosLosGastosFiltradosPorCategoria(idUsuario),
                MesSeleccionado = mes,
                AñoSeleccionado = año
            };

            return View("VerTodosLosGastos", model);
        }

        // Filtro por rango de fechas
        [HttpGet]
        public async Task<IActionResult> FiltrarPorFechas(DateTime inicio, DateTime fin)
        {
            int? idUsuarioLogueado = HttpContext.Session.GetInt32("UsuarioId");
            if (idUsuarioLogueado == null) return RedirectToAction("Login", "Usuario");

            int idUsuario = idUsuarioLogueado.Value;

            var model = new GastoPorCategoriaViewModel
            {
                UltimosGastos = await _servicio.ObtenerGastosPorRangoDeFechasAsync(
                                    idUsuario,
                                    DateOnly.FromDateTime(inicio),
                                    DateOnly.FromDateTime(fin)),
                TotalesPorCategoria = await _servicio.ObtenerTodosLosGastosFiltradosPorCategoria(idUsuario),
                FechaInicio = inicio,
                FechaFin = fin
            };

            return View("VerTodosLosGastos", model);
        }

        public async Task<IActionResult> MostrarGastosPresupuesto(int mes, int anio)
        {
            int? idUsuarioLoguedo = HttpContext.Session.GetInt32("UsuarioId");
            int idUsuario = idUsuarioLoguedo.Value;
            var detalleGastos = await _servicio.ObtenerGastosPorMesAsync(idUsuario, mes, anio);
            await this.FiltrarPorMes(mes, anio);
            return View("VerTodosLosGastos");
        }
    }
}