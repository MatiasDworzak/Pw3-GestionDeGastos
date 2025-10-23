using System;
using System.Collections.Generic;

namespace GestionDeGastos;

public partial class Gasto
{
    public int IdGasto { get; set; }

    public int IdUsuario { get; set; }

    public int? IdTicket { get; set; }

    public int IdMetodoPago { get; set; }

    public int IdCategoria { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal MontoTotal { get; set; }

    public DateOnly Fecha { get; set; }

    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;

    public virtual MetodoDePago IdMetodoPagoNavigation { get; set; } = null!;

    public virtual Ticket? IdTicketNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
