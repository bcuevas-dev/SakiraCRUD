using System.ComponentModel.DataAnnotations;

namespace SakilaWebCRUD.ViewModels
{
    public class LanguageViewModel
    {
        public byte LanguageId { get; set; }

        [Required(ErrorMessage = "El nombre del idioma es obligatorio.")]
        [Display(Name = "Idioma")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Última Actualización")]
        public DateTime LastUpdate { get; set; }
    }
}
