using System;
using System.Collections.Generic;

namespace GestionDeGastos;

public partial class Presupuesto
{
    public int IdPresupuesto { get; set; }

    public int IdUsuario { get; set; }

    public decimal? MontoLimite { get; set; }

    public decimal? MontoActualGastado { get; set; }

    public int Anio { get; set; }

    public int Mes { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
