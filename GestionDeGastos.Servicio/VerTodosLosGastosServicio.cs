using GestionDeGastos.AccesoADatos.Entidades;
using GestionDeGastos.Repositorio;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GestionDeGastos.Servicio
{
    public interface IVerTodosLosGastos
    {
        Task<List<Gasto>> ObtenerTodosLosGastos (int idUsuario);
        Task<List<Gasto>> ObtenerLosGastosFiltradosPorMes(int idUsuario, int mes, int anio);
        Task<List<Gasto>> ObtenerLosGastosFiltradosPorFechasEspecificas(int idUsuario);
        Task<List<Gasto>> ObtenerTodosLosGastosFiltradosPorCategoria(int idUsuario);

    }

    public class VerTodosLosGastosServicio : IVerTodosLosGastos
    {

        private readonly IGastoRepositorio _gastoRepositorio;

        public VerTodosLosGastosServicio(IGastoRepositorio puchi)
        {
            _gastoRepositorio   = puchi;
        }

        public Task<List<Gasto>> ObtenerLosGastosFiltradosPorFechasEspecificas(int idUsuario)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Gasto>> ObtenerLosGastosFiltradosPorMes(int idUsuario, int mes, int anio)
        {
            return await _gastoRepositorio.ObtenerGastosPorMesAsync(mes, anio);
        }

        public async Task<List<Gasto>> ObtenerTodosLosGastos(int idUsuario)
        {
           return  await _gastoRepositorio.ObtenerGastosPorUsuarioAsync(idUsuario);
        }

        public async Task<List<Gasto>> ObtenerTodosLosGastosFiltradosPorCategoria(int idUsuario)
        {
            return  await _gastoRepositorio.ObtenerGastosTotalesPorCategoriaAsync(idUsuario);
        }
    }
}

