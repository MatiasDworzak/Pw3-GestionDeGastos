using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GestionDeGastos.Models.Gasto
{
    public class AgregarGastoViewModel
    {
        // Para recibir por parte del usuario
        [Required(ErrorMessage = "Debe seleccionar una opción de ticket.")]
        public string OpcionTicketSeleccionada { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0.")]
        public decimal? MontoTotal { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateOnly Fecha { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public int? CategoriaSeleccionada { get; set; }

        [Required(ErrorMessage = "El método de pago es obligatorio.")]
        public int? MetodoDePagoSeleccionado { get; set;}

        [Required(ErrorMessage = "La foto es obligatoria.")]
        public IFormFile? TicketFoto { get; set; }

        // Para enviar al usuario
        public List<SelectListItem>? Categorias { get; set; }
        public List<SelectListItem>? MetodosDePago { get; set; }

        // Para que el usuario envie y reciba
        public List<AgregarGastoItemViewModel>? Items { get; set; }
    }

    public class AgregarGastoItemViewModel
    {
        [Required(ErrorMessage = "La descripción del item es obligatoria.")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int? Cantidad { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal? PrecioUnitario { get; set; }
    }
}
