using System;
using System.Collections.Generic;

namespace GestionDeGastos;

public partial class Ticket
{
    public int IdTicket { get; set; }

    public string? RutaImagenBlob { get; set; }

    public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
