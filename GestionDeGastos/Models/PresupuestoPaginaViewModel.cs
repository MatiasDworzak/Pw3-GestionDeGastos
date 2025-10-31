namespace GestionDeGastos.Models
{
    public class PresupuestoPaginaViewModel
    {
        public PresupuestoViewModel UltimoPresupuesto { get; set; }
        public List<PresupuestoViewModel> ListaPresupuestos { get; set; }
        public decimal PorcentajeGastado { get; set; }
    }
}
