using System;
using System.Collections.Generic;

namespace GestionDeGastos;

public partial class CategoriaUsuario
{
    public int IdCategoria { get; set; }

    public int IdUsuario { get; set; }

    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
