using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SakilaWebCRUD.ViewModels
{
    public class CityViewModel
    {
        [Display(Name = "ID")]
        public ushort CityId { get; set; }

        [Required(ErrorMessage = "El nombre de la ciudad es obligatorio.")]
        [Display(Name = "Nombre de Ciudad")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        public string CityName { get; set; } = null!;

        [Required(ErrorMessage = "Debe seleccionar un país.")]
        [Display(Name = "País")]
        public ushort CountryId { get; set; }

        [Display(Name = "Última Actualización")]
        public DateTime LastUpdate { get; set; }

        // Nombre del país (solo para visualización)
        public string CountryName { get; set; } = string.Empty;

        // Lista de países para el combo
        public IEnumerable<SelectListItem>? Paises { get; set; }
    }
}
