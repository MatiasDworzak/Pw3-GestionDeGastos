using System;
using System.Collections.Generic;

namespace GestionDeGastos.AccesoADatos.Entidades;

public partial class MetodoDePago
{
    public int IdMetodoPago { get; set; }

    public string Descripcion { get; set; } = null!;

    public string Icono { get; set; } = null!;

    public string Color { get; set; } = null!;

    public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
}
