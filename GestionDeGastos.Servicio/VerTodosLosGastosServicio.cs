using GestionDeGastos.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeGastos.Servicio
{
    public interface IVerTodosLosGastos
    {
        Task<List<Gasto>> ObtenerTodosLosGastosOrdenadoPorFechaDescendenteAsync(int idUsuario);
     
    }

    public class VerTodosLosGastosServicio : IVerTodosLosGastos
    {


        private readonly IGastoRepositorio _gastoRepositorio;



        public VerTodosLosGastosServicio(IGastoRepositorio gastoRepositorio)
        {
            _gastoRepositorio = gastoRepositorio;
        }
        public async Task<List<Gasto>> ObtenerTodosLosGastosOrdenadoPorFechaDescendenteAsync(int idUsuario)
        {
            return await _gastoRepositorio.ObtenerGastosPorUsuarioAsync(idUsuario);

        }
      

    }
}

