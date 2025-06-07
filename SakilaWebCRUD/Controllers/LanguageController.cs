using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SakilaWebCRUD.Datos;
using SakilaWebCRUD.Models;
using SakilaWebCRUD.ViewModels;

namespace SakilaWebCRUD.Controllers
{
    public class LanguageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LanguageController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Languages
                .Select(l => new
                {
                    l.LanguageId,
                    l.Name,
                    LastUpdate = l.LastUpdate.ToString("yyyy-MM-dd HH:mm:ss")
                })
                .ToListAsync();

            return Json(new { data });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new LanguageViewModel();
            return PartialView("_CreateEditModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(LanguageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var language = new Language
                {
                    Name = model.Name,
                    LastUpdate = DateTime.UtcNow
                };

                _context.Languages.Add(language);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Idioma creado correctamente." });
            }

            return PartialView("_CreateEditModal", model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(byte id)
        {
            var language = await _context.Languages.FindAsync(id);
            if (language == null) return NotFound();

            var model = new LanguageViewModel
            {
                LanguageId = language.LanguageId,
                Name = language.Name,
                LastUpdate = language.LastUpdate
            };

            return PartialView("_CreateEditModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(LanguageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var language = await _context.Languages.FindAsync(model.LanguageId);
                if (language == null) return NotFound();

                language.Name = model.Name;
                language.LastUpdate = DateTime.UtcNow;

                _context.Update(language);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Idioma actualizado correctamente." });
            }

            return PartialView("_CreateEditModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(byte id)
        {
            try
            {
                var language = await _context.Languages.FindAsync(id);
                if (language == null) return NotFound();

                _context.Languages.Remove(language);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Idioma eliminado correctamente." });
            }
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException sql && sql.Number == 1451)
            {
                return Json(new { success = false, message = "No se puede eliminar el idioma porque tiene registros relacionados." });
            }
        }
    }
}
