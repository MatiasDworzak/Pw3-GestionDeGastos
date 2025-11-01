using GestionDeGastos.AccesoADatos.Entidades;

namespace GestionDeGastos.Models.GastoModels
{
    public class GastoViewModel
    {
        public List<Gasto> ListaUltimosTresGastos { get; set; }

        public decimal? Porcentaje { get; set; }
    }
}
