namespace GestionDeGastos.Models
{
    public class GastoEspecificoViewModel
    {
        public string Nombre { get; set; }
        public decimal Monto { get; set; }
        public string Categoria { get; set; }
        public string MetodoDePago { get; set; }
        public DateOnly Fecha { get; set; }
        public string URLFoto { get; set; }

        public List<ItemGastoEspecificoViewModel> Items { get; set; }
    }

    public class ItemGastoEspecificoViewModel
    {
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal PrecioTotal { get; set; }
    }
}
