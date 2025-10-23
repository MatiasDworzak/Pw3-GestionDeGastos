using System;
using System.Collections.Generic;

namespace GestionDeGastos;

public partial class Categorium
{
    public int IdCategoria { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual CategoriaUsuario? CategoriaUsuario { get; set; }

    public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
}
