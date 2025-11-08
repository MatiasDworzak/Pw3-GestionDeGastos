using GestionDeGastos.AccesoADatos.Entidades;
using GestionDeGastos.Repositorio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionDeGastos.Servicio
{
    public interface IVerTodosLosGastos
    {
        Task<List<Gasto>> ObtenerTodosLosGastos(int idUsuario);
        Task<List<Gasto>> ObtenerTodosLosGastosFiltradosPorCategoria(int idUsuario);
        Task<List<Gasto>> ObtenerGastosPorMesAsync(int idUsuario, int mes, int año);
        Task<List<Gasto>> ObtenerGastosPorRangoDeFechasAsync(int idUsuario, DateOnly inicio, DateOnly fin);
    }

    public class VerTodosLosGastosServicio : IVerTodosLosGastos
    {
        private readonly IGastoRepositorio _gastoRepositorio;

        public VerTodosLosGastosServicio(IGastoRepositorio gastoRepositorio)
        {
            _gastoRepositorio = gastoRepositorio;
        }

        public async Task<List<Gasto>> ObtenerTodosLosGastos(int idUsuario)
        {
            return await _gastoRepositorio.ObtenerGastosPorUsuarioAsync(idUsuario);
        }

        public async Task<List<Gasto>> ObtenerTodosLosGastosFiltradosPorCategoria(int idUsuario)
        {
            return await _gastoRepositorio.ObtenerGastosTotalesPorCategoriaAsync(idUsuario);
        }

        public async Task<List<Gasto>> ObtenerGastosPorMesAsync(int idUsuario, int mes, int año)
        {
            return await _gastoRepositorio.ObtenerGastosPorMesAsync(idUsuario, mes, año)
                .ContinueWith(t => t.Result.FindAll(g => g.IdUsuario == idUsuario));
        }

        public async Task<List<Gasto>> ObtenerGastosPorRangoDeFechasAsync(int idUsuario, DateOnly inicio, DateOnly fin)
        {
            return await _gastoRepositorio.ObtenerGastosPorRangoDeFechasAsync(idUsuario, inicio, fin)
                .ContinueWith(t => t.Result.FindAll(g => g.IdUsuario == idUsuario));
        }
    }
}