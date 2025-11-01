using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionDeGastos.AccesoADatos.Entidades;
using GestionDeGastos.Repositorio;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace GestionDeGastos.Servicio
{
    public interface IHomeService
    {
        Task<Usuario?> ObtenerUsuarioPorIdAsync(int id);
        Task<List<Gasto?>> ObtenerUltimosTresGastosPorIdDeUsuario(int id);
        Task<Presupuesto?> ObtenerPresupuestoPorIdDeUsuarioAsync(int id);
        Task<List<Gasto>> ObtenerLosGastosFiltradosPorMes(int? idUsuario, int mes, int año);
        Task<List<Gasto>> ObtenerGastosPorRangoDeFechasAsync(int? idUsuario, DateOnly? fechaInicio, DateOnly? fechaFin);

        decimal ObtenerPresupuestoConPorcentaje(int idUsuario);
        
    }
    public class HomeServicio : IHomeService
    {
        private readonly IPresupuestoRepositorio _presupuestoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IGastoRepositorio _gastoRepositorio;

        public HomeServicio(IUsuarioRepositorio iUsuarioRepo, IPresupuestoRepositorio iPresupuestoRepo, IGastoRepositorio igastoRepositorio)
        {
            _presupuestoRepositorio = iPresupuestoRepo;
            _usuarioRepositorio = iUsuarioRepo;
            _gastoRepositorio = igastoRepositorio;
        }

        public async Task<Presupuesto?> ObtenerPresupuestoPorIdDeUsuarioAsync(int id)
        {
            return await _presupuestoRepositorio.GetByIdAsync(id);
            
        }
        public  decimal  ObtenerPresupuestoConPorcentaje(int idUsuario)
        {
            var presupuesto =  _presupuestoRepositorio.ObtenerUltimoPresupuestoAsync(idUsuario).Result;
            if (presupuesto == null || presupuesto.MontoLimite == 0)
            {
                return 0;
            }

            var porcentajeGastado = (presupuesto.MontoActualGastado / presupuesto.MontoLimite) * 100;
            return Math.Min((decimal)porcentajeGastado, 100);
        }
        public async Task<Usuario?> ObtenerUsuarioPorIdAsync(int id)
        {
            return await _usuarioRepositorio.GetByIdAsync(id);
        }

        public async Task<List<Gasto?>> ObtenerUltimosTresGastosPorIdDeUsuario(int id)
        {
            return await _gastoRepositorio.ObtenerUltimosTresGastosPorUsuarioAsync(id);
        }

        public async Task<List<Gasto>> ObtenerLosGastosFiltradosPorMes(int? idUsuario, int mes, int año)
        {
            return await _gastoRepositorio.ObtenerGastosPorMesAsync(idUsuario, mes, año);
        }

        public async Task<List<Gasto>> ObtenerGastosPorRangoDeFechasAsync(int? idUsuario, DateOnly? fechaInicio, DateOnly? fechaFin)
        {
            return await _gastoRepositorio.ObtenerGastosPorRangoDeFechasAsync(idUsuario, fechaInicio, fechaFin);
        }
    }
}
