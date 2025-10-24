using GestionDeGastos.Repositorio;

namespace GestionDeGastos.Servicio
{

   public interface IUsuarioService
   {
      Task<Usuario?> ObtenerUsuarioPorIdAsync(int id);
      Task<IEnumerable<Usuario>> ObtenerTodosLosUsuariosAsync();
   }

   public class UsuarioServicio : IUsuarioService
   {
      private readonly IUsuarioRepositorio _repositorio;

      public UsuarioServicio(IUsuarioRepositorio repoitorio)
      {
         _repositorio = repoitorio;
      }
      public async Task<IEnumerable<Usuario>> ObtenerTodosLosUsuariosAsync()
      {
         return await _repositorio.GetAllAsync();
      }
      public async Task<Usuario?> ObtenerUsuarioPorIdAsync(int id)
      {
         return await _repositorio.GetByIdAsync(id);
      }
   }

}
