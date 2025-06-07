using System.ComponentModel.DataAnnotations;

namespace SakilaWebCRUD.Models.ViewModels
{
    public class CategoryViewModel
    {
        public byte CategoryId { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [StringLength(25, ErrorMessage = "El nombre no puede superar los 25 caracteres.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;

        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
    }
}
