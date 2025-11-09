using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionDeGastos.AccesoADatos.Entidades;
using GestionDeGastos.Repositorio;

namespace GestionDeGastos.Servicio
{
    public interface IHomeService
    {
        Task<Usuario?> ObtenerUsuarioPorIdAsync(int id);
        Task<List<Gasto?>> ObtenerUltimosCincoGastosPorIdDeUsuario(int id);
        Task<Presupuesto?> ObtenerPresupuestoPorIdDeUsuarioAsync(int id);
        Task<List<Gasto>> ObtenerLosGastosFiltradosPorMes(int? idUsuario, int mes, int año);
        Task<List<Gasto>> ObtenerGastosPorRangoDeFechasAsync(int? idUsuario, DateOnly? fechaInicio, DateOnly? fechaFin);

        
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
    
        public async Task<Usuario?> ObtenerUsuarioPorIdAsync(int id)
        {
            return await _usuarioRepositorio.GetByIdAsync(id);
        }

        public async Task<List<Gasto?>> ObtenerUltimosCincoGastosPorIdDeUsuario(int id)
        {
            return await _gastoRepositorio.ObtenerUltimosCincoGastosPorUsuarioAsync(id);
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
