using GestionDeGastos.AccesoADatos.Entidades;
using Microsoft.EntityFrameworkCore;

namespace GestionDeGastos.Repositorio
{
    public interface ICategoriaRepositorio
    {
        Task<IEnumerable<Categorium>> ObtenerTodasLasCategoriasPorUsuarioAsync(int idUsuario);
    }
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly GestionDeGastosBdContext _dbContext;

        public CategoriaRepositorio(GestionDeGastosBdContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Categorium>> ObtenerTodasLasCategoriasPorUsuarioAsync(int idUsuario)
        {
            return await _dbContext.Categoria
                                    .Where(c => !c.IdUsuarios.Any() ||
                                                c.IdUsuarios.Any(u => u.IdUsuario == idUsuario))
                                    .ToListAsync();
        }
    }
}
