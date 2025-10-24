namespace GestionDeGastos.Models
{
    public class PresupuestoViewModel
    {
        public decimal MontoLimite { get; set; }

        public decimal MontoActualGastado { get; set; }

        public int Anio { get; set; }

        public int Mes { get; set; }
    }
}
