using System;
using System.Collections.Generic;

namespace GestionDeGastos;

public partial class MetodoDePago
{
    public int IdMetodoPago { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
}
