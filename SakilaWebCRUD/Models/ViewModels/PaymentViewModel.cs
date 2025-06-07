using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SakilaWebCRUD.ViewModels
{
    public class PaymentViewModel
    {
        [Display(Name = "ID")]
        public ushort PaymentId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un cliente.")]
        [Display(Name = "Cliente")]
        public ushort CustomerId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un empleado.")]
        [Display(Name = "Empleado")]
        public byte StaffId { get; set; }

        [Display(Name = "Renta")]
        public int? RentalId { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(0.01, 9999.99, ErrorMessage = "El monto debe ser mayor que cero.")]
        [Display(Name = "Monto")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Debe especificar la fecha de pago.")]
        [Display(Name = "Fecha de Pago")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Última Actualización")]
        public DateTime? LastUpdate { get; set; }

        // Visualización
        public string CustomerName { get; set; } = string.Empty;
        public string StaffName { get; set; } = string.Empty;
        public string? RentalInfo { get; set; }

        // Listas desplegables
        public IEnumerable<SelectListItem>? Clientes { get; set; }
        public IEnumerable<SelectListItem>? Empleados { get; set; }
        public IEnumerable<SelectListItem>? Rentas { get; set; }
    }
}
