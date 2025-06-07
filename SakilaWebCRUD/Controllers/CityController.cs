using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SakilaWebCRUD.Datos;
using SakilaWebCRUD.Models;
using SakilaWebCRUD.ViewModels;

namespace SakilaWebCRUD.Controllers
{
    public class CityController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CityController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCities()
        {
            try
            {
                var cities = await _context.Cities
                    .Include(c => c.Country)
                    .Select(c => new
                    {
                        c.CityId,
                        CityName = c.City1,
                        CountryName = c.Country.Country1,
                        LastUpdate = c.LastUpdate.ToString("yyyy-MM-dd HH:mm:ss")
                    }).ToListAsync();

                return Json(new { data = cities });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener las ciudades.", detail = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CityViewModel
            {
                Paises = await GetPaisesAsync()
            };
            return PartialView("_CreateEditModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CityViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var city = new City
                    {
                        City1 = model.CityName,
                        CountryId = model.CountryId,
                        LastUpdate = DateTime.UtcNow
                    };

                    _context.Cities.Add(city);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Ciudad creada correctamente." });
                }

                model.Paises = await GetPaisesAsync();
                return PartialView("_CreateEditModal", model);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al crear la ciudad.", detail = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(ushort id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null) return NotFound();

            var model = new CityViewModel
            {
                CityId = city.CityId,
                CityName = city.City1,
                CountryId = city.CountryId,
                LastUpdate = city.LastUpdate,
                Paises = await GetPaisesAsync()
            };

            return PartialView("_CreateEditModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CityViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var city = await _context.Cities.FindAsync(model.CityId);
                    if (city == null) return NotFound();

                    city.City1 = model.CityName;
                    city.CountryId = model.CountryId;
                    city.LastUpdate = DateTime.UtcNow;

                    _context.Update(city);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Ciudad actualizada correctamente." });
                }

                model.Paises = await GetPaisesAsync();
                return PartialView("_CreateEditModal", model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { success = false, message = "Conflicto de concurrencia al actualizar." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error inesperado al editar.", detail = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ushort id)
        {
            try
            {
                var city = await _context.Cities.FindAsync(id);
                if (city == null) return NotFound();

                _context.Cities.Remove(city);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Ciudad eliminada correctamente." });
            }
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException sqlEx && sqlEx.Number == 1451)
            {
                return Json(new { success = false, message = "No se puede eliminar la ciudad porque tiene registros relacionados." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error inesperado al eliminar.", detail = ex.Message });
            }
        }

        private async Task<List<SelectListItem>> GetPaisesAsync()
        {
            return await _context.Countries
                .OrderBy(c => c.Country1)
                .Select(c => new SelectListItem
                {
                    Value = c.CountryId.ToString(),
                    Text = c.Country1
                }).ToListAsync();
        }
    }
}
