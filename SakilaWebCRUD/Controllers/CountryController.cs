using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SakilaWebCRUD.Datos;
using SakilaWebCRUD.Models;
using SakilaWebCRUD.ViewModels;

namespace SakilaWebCRUD.Controllers
{
    public class CountryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CountryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var countries = await _context.Countries
                .Select(c => new {
                    c.CountryId,
                    Nombre = c.Country1,
                    LastUpdate = c.LastUpdate.ToString("yyyy-MM-dd HH:mm:ss")
                })
                .ToListAsync();

            return Json(new { data = countries });
        }

        [HttpGet]
        public IActionResult Create() => PartialView("_CreateEditModal", new CountryViewModel());

        [HttpPost]
        public async Task<IActionResult> Create(CountryViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("_CreateEditModal", model);

            var country = new Country
            {
                Country1 = model.Nombre,
                LastUpdate = DateTime.UtcNow
            };

            _context.Countries.Add(country);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "País creado correctamente." });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(ushort id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null) return NotFound();

            var model = new CountryViewModel
            {
                CountryId = country.CountryId,
                Nombre = country.Country1,
                LastUpdate = country.LastUpdate
            };

            return PartialView("_CreateEditModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CountryViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("_CreateEditModal", model);

            var country = await _context.Countries.FindAsync(model.CountryId);
            if (country == null) return NotFound();

            country.Country1 = model.Nombre;
            country.LastUpdate = DateTime.UtcNow;

            _context.Update(country);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "País actualizado correctamente." });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ushort id)
        {
            try
            {
                var country = await _context.Countries.FindAsync(id);
                if (country == null) return NotFound();

                _context.Countries.Remove(country);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "País eliminado correctamente." });
            }
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException sqlEx && sqlEx.Number == 1451)
            {
                return Json(new { success = false, message = "No se puede eliminar el país porque tiene ciudades asociadas." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error inesperado al eliminar.", detail = ex.Message });
            }
        }
    }
}
