using System;
using System.Collections.Generic;

namespace GestionDeGastos;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Contrasenia { get; set; } = null!;

    public virtual ICollection<CategoriaUsuario> CategoriaUsuarios { get; set; } = new List<CategoriaUsuario>();

    public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();

    public virtual ICollection<Presupuesto> Presupuestos { get; set; } = new List<Presupuesto>();
}
