namespace GestionDeGastos.Models
{
    public class TicketEscaneadoViewModel
    {
        //public double MontoTotal { get; set; } es necesario?
        public DateOnly FechaEscaneada { get; set; }
        double ConfianzaTotal { get; set; }
        public List<TicketEscaneadoItemViewModel> ItemsEscaneados { get; set; }
    }

    public class TicketEscaneadoItemViewModel
    {
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public double Confianza { get; set; }
    }
}
