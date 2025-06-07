using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SakilaWebCRUD.Models.ViewModels
{
    public class FilmViewModel
    {
        public ushort FilmId { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(128)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [Display(Name = "Año de estreno")]
        public short? ReleaseYear { get; set; }

        [Required(ErrorMessage = "El idioma es obligatorio")]
        [Display(Name = "Idioma")]
        public byte LanguageId { get; set; }

        [Display(Name = "Idioma original")]
        public byte? OriginalLanguageId { get; set; }

        [Display(Name = "Duración de alquiler (días)")]
        public byte RentalDuration { get; set; } = 3;

        [Display(Name = "Precio de alquiler")]
        public decimal RentalRate { get; set; } = 4.99m;

        [Display(Name = "Duración (minutos)")]
        public ushort? Length { get; set; }

        [Display(Name = "Costo de reemplazo")]
        public decimal ReplacementCost { get; set; } = 19.99m;

        [Display(Name = "Clasificación")]
        public string? Rating { get; set; }

        public DateTime LastUpdate { get; set; } = DateTime.Now;

        // Relaciones muchos-a-muchos
        public List<ushort> ActoresSeleccionados { get; set; } = new();
        public List<byte> CategoriasSeleccionadas { get; set; } = new();

        // Listas dinámicas
        public List<SelectListItem> Idiomas { get; set; } = new();
        public List<SelectListItem> IdiomasOriginales { get; set; } = new();
        public List<SelectListItem> ActoresDisponibles { get; set; } = new();
        public List<SelectListItem> CategoriasDisponibles { get; set; } = new();
    }
}
