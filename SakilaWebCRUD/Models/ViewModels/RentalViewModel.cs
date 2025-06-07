using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SakilaWebCRUD.ViewModels
{
    public class RentalViewModel
    {
        public int RentalId { get; set; }

        [Required]
        [Display(Name = "Fecha de Alquiler")]
        public DateTime RentalDate { get; set; }

        [Required]
        [Display(Name = "Película")]
        public uint InventoryId { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public ushort CustomerId { get; set; }

        [Display(Name = "Fecha Devolución")]
        public DateTime? ReturnDate { get; set; }

        [Required]
        [Display(Name = "Empleado")]
        public byte StaffId { get; set; }

        [Display(Name = "Última Actualización")]
        public DateTime LastUpdate { get; set; }

        // Para mostrar en tabla
        public string FilmTitle { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string StaffName { get; set; } = string.Empty;

        // Listas para selects
        public IEnumerable<SelectListItem>? Inventarios { get; set; }
        public IEnumerable<SelectListItem>? Clientes { get; set; }
        public IEnumerable<SelectListItem>? Empleados { get; set; }
    }
}
