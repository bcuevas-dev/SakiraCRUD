using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SakilaWebCRUD.ViewModels;

public class CustomerViewModel
{
    public ushort CustomerId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [Display(Name = "Nombre")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [Display(Name = "Apellido")]
    public string LastName { get; set; } = null!;

    [EmailAddress(ErrorMessage = "Ingrese un correo válido.")]
    [Display(Name = "Correo Electrónico")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Seleccione una tienda.")]
    [Display(Name = "Tienda")]
    public byte StoreId { get; set; }

    [Required(ErrorMessage = "Seleccione una dirección.")]
    [Display(Name = "Dirección")]
    public ushort AddressId { get; set; }

    [Display(Name = "Activo")]
    public bool Active { get; set; }

    //  Fechas opcionales
    [Display(Name = "Fecha de Creación")]
    public DateTime CreateDate { get; set; }

    [Display(Name = "Última Actualización")]
    public DateTime? LastUpdate { get; set; }

    public List<SelectListItem>? Tiendas { get; set; }
    public List<SelectListItem>? Direcciones { get; set; }
}
