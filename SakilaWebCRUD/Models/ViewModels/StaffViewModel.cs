using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SakilaWebCRUD.ViewModels
{
    public class StaffViewModel
    {
        public byte StaffId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [Display(Name = "Apellido")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar una dirección.")]
        [Display(Name = "Dirección")]
        public ushort AddressId { get; set; }

        [Display(Name = "Correo Electrónico")]
        [EmailAddress(ErrorMessage = "Correo no válido.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una tienda.")]
        [Display(Name = "Tienda")]
        public byte StoreId { get; set; }

        [Display(Name = "Activo")]
        public bool? Active { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [Display(Name = "Usuario")]
        public string Username { get; set; } = string.Empty;

        [Display(Name = "Contraseña")]
        public string? Password { get; set; }

        public DateTime LastUpdate { get; set; }

        // Para mostrar en el listado
        public string? AddressDescription { get; set; }
        public string? StoreDescription { get; set; }

        // Listas para los combos
        public IEnumerable<SelectListItem>? Direcciones { get; set; }
        public IEnumerable<SelectListItem>? Tiendas { get; set; }
    }
}
