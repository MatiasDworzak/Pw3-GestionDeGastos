using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GestionDeGastos.Models
{
   public class RegistroViewModel
   {
      [Required(ErrorMessage = "El nombre es obligatorio")]
      public string Nombre { get; set; }
      [Required(ErrorMessage = "El correo electrónico es obligatorio")]

      public string Correo { get; set; }
      [DisplayName("Contraseña")]
      [Required(ErrorMessage = "Ingresá la contraseña")]

      public string Contrasenia { get; set; }

      [DisplayName("Confirmar Contraseña")]
      [Required(ErrorMessage ="Por favor, confirmá tu contraseña")]
      public string ConfirmarContrasenia { get; set; }

   }
}
