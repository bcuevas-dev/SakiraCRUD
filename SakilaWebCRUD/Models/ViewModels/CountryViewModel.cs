using System;
using System.ComponentModel.DataAnnotations;

namespace SakilaWebCRUD.ViewModels
{
    public class CountryViewModel
    {
        [Display(Name = "ID")]
        public ushort CountryId { get; set; }

        [Required(ErrorMessage = "El nombre del país es obligatorio.")]
        [Display(Name = "Nombre del país")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        public string Nombre { get; set; } = null!;

        [Display(Name = "Última Actualización")]
        public DateTime LastUpdate { get; set; }
    }
}
