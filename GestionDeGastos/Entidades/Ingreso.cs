using System;
using System.Collections.Generic;

namespace GestionDeGastos.Entidades;

public partial class Ingreso
{
    public int IdIngreso { get; set; }

    public int IdUsuario { get; set; }

    public decimal Monto { get; set; }

    public DateOnly Fecha { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
