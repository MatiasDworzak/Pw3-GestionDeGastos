namespace GestionDeGastos.Servicio.DTO
{
    public class TicketEscaneadoDTO
    {
        //public double MontoTotal { get; set; } es necesario?
        public DateOnly FechaEscaneada { get; set; }
        public decimal? Iva { get; set; }
        public decimal? Descuento { get; set; }
        public List<TicketEscaneadoItemDTO> ItemsEscaneados { get; set; } = new List<TicketEscaneadoItemDTO>();
    }

    public class TicketEscaneadoItemDTO
    {
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
