using System;
using System.Collections.Generic;

namespace GestionDeGastos.AccesoADatos.Entidades;

public partial class Categorium
{
    public int IdCategoria { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();

    public virtual ICollection<Usuario> IdUsuarios { get; set; } = new List<Usuario>();
}
