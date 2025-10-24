

using GestionDeGastos.Servicio.DTOs;

namespace Validacion
{
   public static class ValidacionUsuario
   {
      public static void Registro(RegistrarUsuarioDto model)
      {
         if (model == null) throw new ArgumentNullException(nameof(model), "El modelo de registro no puede ser nulo.");

         ValidarStringRequerido(model.Nombre, nameof(model.Nombre));
         ValidarStringRequerido(model.Correo, nameof(model.Correo));
         ValidarStringRequerido(model.Contrasenia, nameof(model.Contrasenia));
      }
      public static void Login(ValidarUsuarioDto model)
      {
         if (model == null) throw new ArgumentNullException(nameof(model), "El modelo de credenciales no puede ser nulo.");

         ValidarStringRequerido(model.Correo, nameof(model.Correo));
         ValidarStringRequerido(model.Contrasenia, nameof(model.Contrasenia));
      }

      private static void ValidarStringRequerido(string valor, string errorDelParametro)
      {
         if (valor == null)
         {
            throw new ArgumentNullException(errorDelParametro, $"El campo '{errorDelParametro}' no puede ser nulo.");
         }
         if (string.IsNullOrWhiteSpace(valor))
         {
            throw new ArgumentException($"El campo '{errorDelParametro}' no puede estar vacío o contener solo espacios.", valor);
         }
      }
   }
}
