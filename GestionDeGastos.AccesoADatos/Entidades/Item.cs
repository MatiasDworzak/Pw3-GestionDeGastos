using System;
using System.Collections.Generic;

namespace GestionDeGastos;

public partial class Item
{
    public int IdItem { get; set; }

    public int IdTicket { get; set; }

    public string Descripcion { get; set; } = null!;

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal PrecioTotal { get; set; }

    public virtual Ticket IdTicketNavigation { get; set; } = null!;
}
