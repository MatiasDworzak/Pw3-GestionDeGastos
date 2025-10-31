using GestionDeGastos.AccesoADatos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeGastos.Repositorio
{
    public interface IGastoRepositorio
    {
        Task AgregarGastoAsync(Gasto gasto);
    }
    public class GastoRepositorio : IGastoRepositorio
    {
        private readonly GestionDeGastosBdContext _dbContext;

        public GastoRepositorio(GestionDeGastosBdContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AgregarGastoAsync(Gasto gasto)
        {
            if (gasto == null) throw new ArgumentNullException(nameof(gasto), "El gasto no puede ser null");

            await _dbContext.Gastos.AddAsync(gasto);
            await _dbContext.SaveChangesAsync();
        }
    }
}
