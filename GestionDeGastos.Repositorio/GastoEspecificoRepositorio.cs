using GestionDeGastos.AccesoADatos;
using System.Data.Entity;

namespace GestionDeGastos.Repositorio
{
    public interface IGastoEspecificoRepositorio
    {
        Task<Gasto> ObtenerGastoPorID(int id);
    }
    public class GastoEspecificoRepositorio : IGastoEspecificoRepositorio
    {
        private readonly GestionDeGastosBdContext _contexto;

        public GastoEspecificoRepositorio(GestionDeGastosBdContext contexto)
        {
            _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
        }

        public async Task<Gasto> ObtenerGastoPorID(int id)
        {
            var gasto = _contexto.Gastos.Include(g => g.IdGasto).FirstOrDefault(g => g.IdGasto == id);
            if (gasto == null)
            {
                throw new Exception("Gasto no encontrado");
            }
            return gasto;
        }
    }
}
