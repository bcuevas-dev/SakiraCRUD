using System.ComponentModel.DataAnnotations;

namespace SakilaWebCRUD.Models.ViewModels
{
    public class ActorViewModel
    {
        public ushort ActorId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(45)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(45)]
        public string LastName { get; set; } = null!;

        public DateTime LastUpdate { get; set; } = DateTime.Now;
    }
}
