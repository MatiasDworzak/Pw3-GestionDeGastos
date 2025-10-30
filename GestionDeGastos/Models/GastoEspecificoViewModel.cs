namespace GestionDeGastos.Models
{
    public class GastoEspecificoViewModel
    {
        public int IdGasto { get; set; }
        public string Nombre { get; set; }
        public decimal Monto { get; set; }
        public CategoriaGastoEspecificoViewModel Categoria { get; set; }
        public MetodoDePagoGastoEspecificoViewModel MetodoDePago { get; set; }
        public DateOnly Fecha { get; set; }
        
        //public string URLFoto { get; set; }

        public TicketGastoEspecificoViewModel? Ticket { get; set; }

        public List<ItemGastoEspecificoViewModel>? Items { get; set; }
    }

    public class ItemGastoEspecificoViewModel
    {
        public int IdItem { get; set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal PrecioTotal { get; set; }
    }
    public class CategoriaGastoEspecificoViewModel
    {
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; }
    }
    public class MetodoDePagoGastoEspecificoViewModel
    {
        public int IdMetodoDePago { get; set; }
        public string NombreMetodoDePago { get; set; }
    }
    public class TicketGastoEspecificoViewModel
    {
        public int IdTicket { get; set; } = 0;
        public string URLFoto { get; set; }
    }
}
