
namespace GestionDeGastos.Servicio.Validacion
{
   public static class ValidacionUsuario
   {
      public static void ValidarRegistro(Usuario usuario)
      {
         if (usuario is null)
         {
            throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo.");
         }

         ValidarCampoRequerido(usuario.Nombre, nameof(usuario.Nombre));
         ValidarCampoRequerido(usuario.Email, nameof(usuario.Email));
         ValidarCampoRequerido(usuario.Contrasenia, nameof(usuario.Contrasenia));
         ValidarFormatoEmail(usuario.Email);
      }

      public static void ValidarCredenciales(string correo, string contrasenia)
      {
         ValidarCampoRequerido(correo, "Correo electrónico");
         ValidarCampoRequerido(contrasenia, "Contraseña");
         ValidarFormatoEmail(correo);
      }

      private static void ValidarCampoRequerido(string valor, string nombreCampo)
      {
         if (valor is null)
         {
            throw new ArgumentNullException(nombreCampo, $"El campo '{nombreCampo}' no puede ser nulo.");
         }

         if (string.IsNullOrWhiteSpace(valor))
         {
            throw new ArgumentException($"El campo '{nombreCampo}' no puede estar vacío.", nombreCampo);
         }
      }

      private static void ValidarFormatoEmail(string email)
      {
         try
         {
            var mailAddress = new System.Net.Mail.MailAddress(email);
         }
         catch
         {
            throw new ArgumentException("El formato del correo electrónico no es válido.", nameof(email));
         }
      }
   }
}