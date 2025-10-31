using System;
using System.Collections.Generic;

namespace GestionDeGastos.AccesoADatos.Entidades;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Contrasenia { get; set; } = null!;

    public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();

    public virtual ICollection<Ingreso> Ingresos { get; set; } = new List<Ingreso>();

    public virtual ICollection<Presupuesto> Presupuestos { get; set; } = new List<Presupuesto>();

    public virtual ICollection<Categorium> IdCategoria { get; set; } = new List<Categorium>();
}
