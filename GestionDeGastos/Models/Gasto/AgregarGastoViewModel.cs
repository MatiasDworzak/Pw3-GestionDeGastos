using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GestionDeGastos.Models.Gasto
{
    public class AgregarGastoViewModel
    {
        // Para recibir por parte del usuario
        [Required]
        public string OpcionTicketSeleccionada { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal? MontoTotal { get; set; }

        [Required]
        public DateOnly Fecha { get; set; }

        [Required]
        public int CategoriaSeleccionada { get; set; }

        [Required]
        public int MetodoDePagoSeleccionado { get; set;}
        // necesitaria ver en que formato mandar la imagen al backend para que la guarde en el blob
        // public IFormFile TicketFoto { get; set; }

        // Para enviar al usuario
        public List<SelectListItem> Categorias { get; set; }
        public List<SelectListItem> MetodosDePago { get; set; }

        // Para que el usuario envie y reciba
        public List<AgregarGastoItemViewModel> Items { get; set; }
    }

    public class AgregarGastoItemViewModel
    {
        public string Descripcion { get; set; }

        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal PrecioUnitario { get; set; }
    }
}
