using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SakilaWebCRUD.Datos;
using SakilaWebCRUD.Models;
using SakilaWebCRUD.ViewModels;

namespace SakilaWebCRUD.Controllers
{
    public class AddressController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AddressController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Addresses
                .Include(a => a.City)
                .Select(a => new
                {
                    a.AddressId,
                    a.Address1,
                    a.Address2,
                    a.District,
                    a.PostalCode,
                    a.Phone,
                    CityName = a.City.City1,
                    LastUpdate = a.LastUpdate.ToString("yyyy-MM-dd HH:mm:ss")
                })
                .ToListAsync();

            return Json(new { data });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new AddressViewModel
            {
                Ciudades = await GetCiudades()
            };
            return PartialView("_CreateEditModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddressViewModel model)
        {
            if (ModelState.IsValid)
            {
                var address = new Address
                {
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    District = model.District,
                    CityId = model.CityId,
                    PostalCode = model.PostalCode,
                    Phone = model.Phone,
                    LastUpdate = DateTime.UtcNow
                };

                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Dirección creada correctamente." });
            }

            model.Ciudades = await GetCiudades();
            return PartialView("_CreateEditModal", model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(ushort id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null) return NotFound();

            var model = new AddressViewModel
            {
                AddressId = address.AddressId,
                Address1 = address.Address1,
                Address2 = address.Address2,
                District = address.District,
                CityId = address.CityId,
                PostalCode = address.PostalCode,
                Phone = address.Phone,
                LastUpdate = address.LastUpdate,
                Ciudades = await GetCiudades()
            };

            return PartialView("_CreateEditModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AddressViewModel model)
        {
            if (ModelState.IsValid)
            {
                var address = await _context.Addresses.FindAsync(model.AddressId);
                if (address == null) return NotFound();

                address.Address1 = model.Address1;
                address.Address2 = model.Address2;
                address.District = model.District;
                address.CityId = model.CityId;
                address.PostalCode = model.PostalCode;
                address.Phone = model.Phone;
                address.LastUpdate = DateTime.UtcNow;

                _context.Update(address);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Dirección actualizada correctamente." });
            }

            model.Ciudades = await GetCiudades();
            return PartialView("_CreateEditModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ushort id)
        {
            try
            {
                var address = await _context.Addresses.FindAsync(id);
                if (address == null) return NotFound();

                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Dirección eliminada correctamente." });
            }
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException sql && sql.Number == 1451)
            {
                return Json(new { success = false, message = "No se puede eliminar la dirección porque tiene registros relacionados." });
            }
        }

        private async Task<List<SelectListItem>> GetCiudades()
        {
            return await _context.Cities
                .OrderBy(c => c.City1)
                .Select(c => new SelectListItem
                {
                    Value = c.CityId.ToString(),
                    Text = c.City1
                })
                .ToListAsync();
        }
    }
}
