using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SakilaWebCRUD.Datos;
using SakilaWebCRUD.Models;
using SakilaWebCRUD.ViewModels;

namespace SakilaWebCRUD.Controllers
{
    public class RentalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RentalController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Rentals
                .Include(r => r.Inventory)
                    .ThenInclude(i => i.Film)
                .Include(r => r.Customer)
                .Include(r => r.Staff)
                .Select(r => new
                {
                    r.RentalId,
                    r.RentalDate,
                    r.ReturnDate,
                    FilmTitle = r.Inventory.Film.Title,
                    CustomerName = r.Customer.FirstName + " " + r.Customer.LastName,
                    StaffName = r.Staff.FirstName + " " + r.Staff.LastName,
                    LastUpdate = r.LastUpdate.ToString("yyyy-MM-dd HH:mm:ss")
                })
                .ToListAsync();

            return Json(new { data });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new RentalViewModel
            {
                RentalDate = DateTime.UtcNow,
                Inventarios = await GetInventarios(),
                Clientes = await GetClientes(),
                Empleados = await GetEmpleados()
            };
            return PartialView("_CreateEditModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RentalViewModel model)
        {
            if (ModelState.IsValid)
            {
                var rental = new Rental
                {
                    RentalDate = model.RentalDate,
                    InventoryId = model.InventoryId,
                    CustomerId = model.CustomerId,
                    ReturnDate = model.ReturnDate,
                    StaffId = model.StaffId,
                    LastUpdate = DateTime.UtcNow
                };

                _context.Rentals.Add(rental);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Alquiler registrado correctamente." });
            }

            model.Inventarios = await GetInventarios();
            model.Clientes = await GetClientes();
            model.Empleados = await GetEmpleados();
            return PartialView("_CreateEditModal", model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null) return NotFound();

            var model = new RentalViewModel
            {
                RentalId = rental.RentalId,
                RentalDate = rental.RentalDate,
                InventoryId = rental.InventoryId,
                CustomerId = rental.CustomerId,
                ReturnDate = rental.ReturnDate,
                StaffId = rental.StaffId,
                LastUpdate = rental.LastUpdate,
                Inventarios = await GetInventarios(),
                Clientes = await GetClientes(),
                Empleados = await GetEmpleados()
            };

            return PartialView("_CreateEditModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RentalViewModel model)
        {
            if (ModelState.IsValid)
            {
                var rental = await _context.Rentals.FindAsync(model.RentalId);
                if (rental == null) return NotFound();

                rental.RentalDate = model.RentalDate;
                rental.InventoryId = model.InventoryId;
                rental.CustomerId = model.CustomerId;
                rental.ReturnDate = model.ReturnDate;
                rental.StaffId = model.StaffId;
                rental.LastUpdate = DateTime.UtcNow;

                _context.Rentals.Update(rental);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Alquiler actualizado correctamente." });
            }

            model.Inventarios = await GetInventarios();
            model.Clientes = await GetClientes();
            model.Empleados = await GetEmpleados();
            return PartialView("_CreateEditModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var rental = await _context.Rentals.FindAsync(id);
                if (rental == null) return NotFound();

                _context.Rentals.Remove(rental);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Alquiler eliminado correctamente." });
            }
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException sql && sql.Number == 1451)
            {
                return Json(new { success = false, message = "No se puede eliminar el alquiler porque tiene pagos relacionados." });
            }
        }

        private async Task<List<SelectListItem>> GetInventarios()
        {
            return await _context.Inventories
                .Include(i => i.Film)
                .OrderBy(i => i.Film.Title)
                .Select(i => new SelectListItem
                {
                    Value = i.InventoryId.ToString(),
                    Text = i.Film.Title
                }).ToListAsync();
        }

        private async Task<List<SelectListItem>> GetClientes()
        {
            return await _context.Customers
                .OrderBy(c => c.FirstName)
                .Select(c => new SelectListItem
                {
                    Value = c.CustomerId.ToString(),
                    Text = c.FirstName + " " + c.LastName
                }).ToListAsync();
        }

        private async Task<List<SelectListItem>> GetEmpleados()
        {
            return await _context.Staff
                .OrderBy(s => s.FirstName)
                .Select(s => new SelectListItem
                {
                    Value = s.StaffId.ToString(),
                    Text = s.FirstName + " " + s.LastName
                }).ToListAsync();
        }
    }
}
