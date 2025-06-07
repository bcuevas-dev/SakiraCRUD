using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SakilaWebCRUD.ViewModels
{
    public class AddressViewModel
    {
        public ushort AddressId { get; set; }

        [Required]
        [Display(Name = "Dirección 1")]
        public string Address1 { get; set; } = string.Empty;

        [Display(Name = "Dirección 2")]
        public string? Address2 { get; set; }

        [Required]
        [Display(Name = "Distrito")]
        public string District { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Ciudad")]
        public ushort CityId { get; set; }

        [Display(Name = "Código Postal")]
        public string? PostalCode { get; set; }

        [Required]
        [Display(Name = "Teléfono")]
        public string Phone { get; set; } = string.Empty;

        public DateTime LastUpdate { get; set; }

        public string CityName { get; set; } = string.Empty;

        public IEnumerable<SelectListItem>? Ciudades { get; set; }
    }
}
