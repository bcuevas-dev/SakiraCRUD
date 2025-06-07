using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SakilaWebCRUD.Datos;
using SakilaWebCRUD.Models;
using SakilaWebCRUD.ViewModels;

namespace SakilaWebCRUD.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                var customers = await _context.Customers
                    .Include(c => c.Store)
                    .Include(c => c.Address)
                        .ThenInclude(a => a.City)
                    .Select(c => new
                    {
                        c.CustomerId,
                        Nombre = c.FirstName + " " + c.LastName,
                        Email = c.Email,
                        Direccion = c.Address.Address1,
                        Ciudad = c.Address.City.City1,
                        Tienda = c.Store.StoreId,
                        Activo = c.Active == true ? "Sí" : "No",
                        FechaCreacion = c.CreateDate.ToString("yyyy-MM-dd")
                    }).ToListAsync();

                return Json(new { data = customers });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener clientes.", detail = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CustomerViewModel
            {
                Tiendas = await GetTiendasAsync(),
                Direcciones = await GetDireccionesAsync()
            };
            return PartialView("_CreateEditModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    StoreId = model.StoreId,
                    AddressId = model.AddressId,
                    Active = model.Active,
                    CreateDate = DateTime.UtcNow,
                    LastUpdate = DateTime.UtcNow
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Cliente creado correctamente." });
            }

            model.Tiendas = await GetTiendasAsync();
            model.Direcciones = await GetDireccionesAsync();
            return PartialView("_CreateEditModal", model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(ushort id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            var model = new CustomerViewModel
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                StoreId = customer.StoreId,
                AddressId = customer.AddressId,
                Active = customer.Active ?? true,
                CreateDate = customer.CreateDate,
                LastUpdate = customer.LastUpdate ?? DateTime.UtcNow,
                Tiendas = await GetTiendasAsync(),
                Direcciones = await GetDireccionesAsync()
            };

            return PartialView("_CreateEditModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = await _context.Customers.FindAsync(model.CustomerId);
                if (customer == null) return NotFound();

                customer.FirstName = model.FirstName;
                customer.LastName = model.LastName;
                customer.Email = model.Email;
                customer.StoreId = model.StoreId;
                customer.AddressId = model.AddressId;
                customer.Active = model.Active;
                customer.LastUpdate = DateTime.UtcNow;

                _context.Update(customer);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Cliente actualizado correctamente." });
            }

            model.Tiendas = await GetTiendasAsync();
            model.Direcciones = await GetDireccionesAsync();
            return PartialView("_CreateEditModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ushort id)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null) return NotFound();

                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Cliente eliminado correctamente." });
            }
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException sqlEx && sqlEx.Number == 1451)
            {
                return Json(new { success = false, message = "No se puede eliminar el cliente porque tiene registros relacionados." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error inesperado al eliminar.", detail = ex.Message });
            }
        }

        private async Task<List<SelectListItem>> GetTiendasAsync()
        {
            return await _context.Stores
                .Select(t => new SelectListItem
                {
                    Value = t.StoreId.ToString(),
                    Text = "Tienda #" + t.StoreId
                }).ToListAsync();
        }

        private async Task<List<SelectListItem>> GetDireccionesAsync()
        {
            return await _context.Addresses
                .Select(a => new SelectListItem
                {
                    Value = a.AddressId.ToString(),
                    Text = a.Address1 + ", " + a.District
                }).ToListAsync();
        }
    }
}
