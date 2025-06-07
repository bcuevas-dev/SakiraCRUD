using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SakilaWebCRUD.Models.ViewModels
{
    public class InventoryViewModel
    {
        public uint InventoryId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una película.")]
        [Display(Name = "Película")]
        public ushort FilmId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una tienda.")]
        [Display(Name = "Tienda")]
        public byte StoreId { get; set; }

        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

        // Dropdowns cargados dinámicamente
        public List<SelectListItem> Peliculas { get; set; } = new();
        public List<SelectListItem> Tiendas { get; set; } = new();
    }
}
