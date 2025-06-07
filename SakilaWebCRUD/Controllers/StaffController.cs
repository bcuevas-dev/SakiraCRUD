using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SakilaWebCRUD.Datos;
using SakilaWebCRUD.Models;
using SakilaWebCRUD.ViewModels;

namespace SakilaWebCRUD.Controllers
{
    public class StaffController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StaffController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View();
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var empleados = await _context.Staff
                .Include(s => s.Address)
                .Include(s => s.Store)
                .Select(s => new
                {
                    staffId = s.StaffId,
                    nombreCompleto = s.FirstName + " " + s.LastName,
                    email = s.Email,
                    username = s.Username,
                    direccion = s.Address.Address1,
                    tienda = "Tienda #" + s.StoreId,
                    activo = s.Active,
                    lastUpdate = s.LastUpdate.ToString("yyyy-MM-dd HH:mm:ss")
                })
                .ToListAsync();

            return Json(new { data = empleados });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new StaffViewModel
            {
                Direcciones = await GetDirecciones(),
                Tiendas = await GetTiendas()
            };
            return PartialView("_CreateEditModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(StaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                var staff = new Staff
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Username = model.Username,
                    Password = model.Password,
                    AddressId = model.AddressId,
                    StoreId = model.StoreId,
                    Active = model.Active,
                    LastUpdate = DateTime.UtcNow
                };

                _context.Staff.Add(staff);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Empleado creado correctamente." });
            }

            model.Direcciones = await GetDirecciones();
            model.Tiendas = await GetTiendas();
            return PartialView("_CreateEditModal", model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(byte id)
        {
            var staff = await _context.Staff.FindAsync(id);
            if (staff == null) return NotFound();

            var model = new StaffViewModel
            {
                StaffId = staff.StaffId,
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                Email = staff.Email,
                Username = staff.Username,
                Password = staff.Password,
                AddressId = staff.AddressId,
                StoreId = staff.StoreId,
                Active = staff.Active,
                LastUpdate = staff.LastUpdate,
                Direcciones = await GetDirecciones(),
                Tiendas = await GetTiendas()
            };

            return PartialView("_CreateEditModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                var staff = await _context.Staff.FindAsync(model.StaffId);
                if (staff == null) return NotFound();

                staff.FirstName = model.FirstName;
                staff.LastName = model.LastName;
                staff.Email = model.Email;
                staff.Username = model.Username;
                staff.Password = model.Password;
                staff.AddressId = model.AddressId;
                staff.StoreId = model.StoreId;
                staff.Active = model.Active;
                staff.LastUpdate = DateTime.UtcNow;

                _context.Update(staff);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Empleado actualizado correctamente." });
            }

            model.Direcciones = await GetDirecciones();
            model.Tiendas = await GetTiendas();
            return PartialView("_CreateEditModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(byte id)
        {
            try
            {
                var staff = await _context.Staff.FindAsync(id);
                if (staff == null) return NotFound();

                _context.Staff.Remove(staff);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Empleado eliminado correctamente." });
            }
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException sql && sql.Number == 1451)
            {
                return Json(new { success = false, message = "No se puede eliminar el empleado porque tiene registros relacionados." });
            }
        }

        private async Task<List<SelectListItem>> GetDirecciones()
        {
            return await _context.Addresses
                .OrderBy(a => a.Address1)
                .Select(a => new SelectListItem
                {
                    Value = a.AddressId.ToString(),
                    Text = a.Address1
                }).ToListAsync();
        }

        private async Task<List<SelectListItem>> GetTiendas()
        {
            return await _context.Stores
                .OrderBy(s => s.StoreId)
                .Select(s => new SelectListItem
                {
                    Value = s.StoreId.ToString(),
                    Text = $"Tienda #{s.StoreId}"
                }).ToListAsync();
        }
    }
}
