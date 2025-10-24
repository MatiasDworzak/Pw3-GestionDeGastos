using GestionDeGastos.Repositorio;
using GestionDeGastos.Servicio.Seguridad;
using GestionDeGastos.Servicio.Validacion;

namespace GestionDeGastos.Servicio
{
   public interface IAutenticacionServicio
   {
      Task<Usuario?> RegistrarUsuarioAsync(Usuario usuario);
      Task<Usuario?> ValidarCredencialesAsync(string correo, string contraseniaPlana);
   }

   public class AutenticacionServicio : IAutenticacionServicio
   {
      private readonly IUsuarioRepositorio _usuarioRepositorio;
      private readonly IContraseniaHasher _hasher;

      public AutenticacionServicio(
          IUsuarioRepositorio usuarioRepositorio,
          IContraseniaHasher hasher)
      {
         _usuarioRepositorio = usuarioRepositorio;
         _hasher = hasher;
      }

      public async Task<Usuario?> RegistrarUsuarioAsync(Usuario nuevoUsuario)
      {
         ValidacionUsuario.ValidarRegistro(nuevoUsuario);

         bool existeUsuario = await ExisteUsuarioPorEmailAsync(nuevoUsuario.Email);
         if (existeUsuario)
         {
            return null;
         }

         string contraseniaHash = _hasher.Hash(nuevoUsuario.Contrasenia);
         nuevoUsuario.Contrasenia = contraseniaHash;

         await _usuarioRepositorio.AddAsync(nuevoUsuario);
         return nuevoUsuario;
      }

      public async Task<Usuario?> ValidarCredencialesAsync(string correo, string contraseniaPlana)
      {
         ValidacionUsuario.ValidarCredenciales(correo, contraseniaPlana);

         var usuario = await _usuarioRepositorio.GetByEmailAsync(correo);
         if (usuario is null)
         {
            return null;
         }

         bool esContraseniaValida = _hasher.Verificar(contraseniaPlana, usuario.Contrasenia);
         return esContraseniaValida ? usuario : null;
      }

      private async Task<bool> ExisteUsuarioPorEmailAsync(string email)
      {
         var usuarioExistente = await _usuarioRepositorio.GetByEmailAsync(email);
         return usuarioExistente is not null;
      }
   }
}