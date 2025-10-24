using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Timers;

namespace GestionDeGastos.Models
{
   public class CredencialViewModel
   {

      [Required(ErrorMessage ="El correo es obligatorio")]
      [DisplayName("Correo electrónico")]
      public string Correo{ get; set; }


      [Required(ErrorMessage ="La contraseña es obligatoria")]
      [DisplayName("Contraseña")]
      public string Contrasenia { get; set; }
   }
}
