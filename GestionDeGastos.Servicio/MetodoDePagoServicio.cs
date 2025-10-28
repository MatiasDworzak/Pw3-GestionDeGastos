using GestionDeGastos.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeGastos.Servicio
{
    public interface IMetodoDePagoServicio
    {
        Task<IEnumerable<MetodoDePago>> ObtenerTodosLosMetodosDePagoAsync();

    }
    public class MetodoDePagoServicio : IMetodoDePagoServicio
    {
        private readonly IMetodoDePagoRepositorio _repositorio;

        public MetodoDePagoServicio(IMetodoDePagoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<IEnumerable<MetodoDePago>> ObtenerTodosLosMetodosDePagoAsync()
        {
            return await _repositorio.ObtenerTodosLosMetodosDePagoAsync();
        }
    }
}
