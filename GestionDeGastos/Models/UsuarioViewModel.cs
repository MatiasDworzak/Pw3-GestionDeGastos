namespace GestionDeGastos.Models
{
   public class UsuarioViewModel
   {
      public int IdUsuario { get; set; }

      public string Nombre { get; set; } = null!;

      public string Email { get; set; } = null!;


      //replazar por ViewModels cada ICollection 

     // public virtual ICollection<CategoriaUsuario> CategoriaUsuarios { get; set; } = new List<CategoriaUsuario>();

      //public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();

     // public virtual ICollection<Presupuesto> Presupuestos { get; set; } = new List<Presupuesto>();
   }
}
