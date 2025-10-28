using GestionDeGastos.AccesoADatos.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeGastos.Repositorio
{
    public interface IMetodoDePagoRepositorio
    {
        Task<IEnumerable<MetodoDePago>> ObtenerTodosLosMetodosDePagoAsync();
    }
    public class MetodoDePagoRepositorio : IMetodoDePagoRepositorio
    {
        private readonly GestionDeGastosBdContext _dbContext;

        public MetodoDePagoRepositorio(GestionDeGastosBdContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<MetodoDePago>> ObtenerTodosLosMetodosDePagoAsync()
        {
            return await _dbContext.MetodoDePagos.ToListAsync();
        }
    }
}
