
using System.Data.Entity;
using System.Threading;

namespace GestionDeGastos.Repositorio
{
   public interface IUsuarioRepositorio
   {
      Task<Usuario?> GetByIdAsync(int id); 
      Task<Usuario?> GetByEmailAsync(string email);
      Task<IEnumerable<Usuario>> GetAllAsync(); 
      Task AddAsync(Usuario usuario);
      Task UpdateAsync(Usuario usuario);
      Task DeleteAsync(Usuario usuario);
   }

   public class UsuarioRepositorio : IUsuarioRepositorio
   {
      private readonly GestionDeGastosBdContext _dbContext;

      public UsuarioRepositorio(GestionDeGastosBdContext context)
      {
         _dbContext = context;

      }

      public async Task<Usuario?> GetByIdAsync(int id)
         => await _dbContext.Usuarios.FindAsync(id);
     

      public async Task<Usuario?> GetByEmailAsync(string email)
         =>await _dbContext.Usuarios
         .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
      

      public async Task<IEnumerable<Usuario>> GetAllAsync()
      {

         return await _dbContext.Usuarios.ToListAsync();
      }

      public async Task AddAsync(Usuario usuario)
      {
         if (usuario == null) throw new ArgumentNullException(nameof(usuario));

         await _dbContext.Usuarios.AddAsync(usuario);
         await _dbContext.SaveChangesAsync(); 
      }

      public async Task UpdateAsync(Usuario usuario)
      {
         if (usuario == null) throw new ArgumentNullException(nameof(usuario));

         _dbContext.Usuarios.Update(usuario);
         await _dbContext.SaveChangesAsync();
      }

      public async Task DeleteAsync(Usuario usuario)
      {
         if (usuario == null) throw new ArgumentNullException(nameof(usuario));

         _dbContext.Usuarios.Remove(usuario);
         await _dbContext.SaveChangesAsync();
      }
   }
}
