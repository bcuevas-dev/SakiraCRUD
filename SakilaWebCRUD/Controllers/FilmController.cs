using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SakilaWebCRUD.Datos;
using SakilaWebCRUD.Models;
using SakilaWebCRUD.Models.ViewModels;

namespace SakilaWebCRUD.Controllers
{
    public class FilmController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FilmController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var films = await _context.Films
                .Include(f => f.Language)
                .Select(f => new
                {
                    f.FilmId,
                    f.Title,
                    f.ReleaseYear,
                    f.Rating,
                    Idioma = f.Language.Name,
                    f.LastUpdate
                }).ToListAsync();

            return Json(new { data = films });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new FilmViewModel
            {
                Idiomas = await GetIdiomasAsync(),
                IdiomasOriginales = await GetIdiomasAsync(true),
                ActoresDisponibles = await GetActoresAsync(),
                CategoriasDisponibles = await GetCategoriasAsync()
            };
            return PartialView("_CreateEditModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FilmViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await CargarListas(model);
                return PartialView("_CreateEditModal", model);
            }

            var film = new Film
            {
                Title = model.Title,
                Description = model.Description,
                ReleaseYear = model.ReleaseYear,
                LanguageId = model.LanguageId,
                OriginalLanguageId = model.OriginalLanguageId,
                RentalDuration = model.RentalDuration,
                RentalRate = model.RentalRate,
                Length = model.Length,
                ReplacementCost = model.ReplacementCost,
                Rating = model.Rating,
                LastUpdate = DateTime.UtcNow
            };

            // Relación Film-Actor
            foreach (var actorId in model.ActoresSeleccionados)
            {
                film.FilmActors.Add(new FilmActor
                {
                    ActorId = actorId,
                    LastUpdate = DateTime.UtcNow
                });
            }

            // Relación Film-Categoría
            foreach (var catId in model.CategoriasSeleccionadas)
            {
                film.FilmCategories.Add(new FilmCategory
                {
                    CategoryId = catId,
                    LastUpdate = DateTime.UtcNow
                });
            }

            _context.Films.Add(film);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Película creada correctamente." });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(ushort id)
        {
            var film = await _context.Films
                .Include(f => f.FilmActors)
                .Include(f => f.FilmCategories)
                .FirstOrDefaultAsync(f => f.FilmId == id);

            if (film == null) return NotFound();

            var model = new FilmViewModel
            {
                FilmId = film.FilmId,
                Title = film.Title,
                Description = film.Description,
                ReleaseYear = film.ReleaseYear,
                LanguageId = film.LanguageId,
                OriginalLanguageId = film.OriginalLanguageId,
                RentalDuration = film.RentalDuration,
                RentalRate = film.RentalRate,
                Length = film.Length,
                ReplacementCost = film.ReplacementCost,
                Rating = film.Rating,
                LastUpdate = film.LastUpdate,
                ActoresSeleccionados = film.FilmActors.Select(fa => fa.ActorId).ToList(),
                CategoriasSeleccionadas = film.FilmCategories.Select(fc => fc.CategoryId).ToList()
            };

            await CargarListas(model);
            return PartialView("_CreateEditModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FilmViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await CargarListas(model);
                return PartialView("_CreateEditModal", model);
            }

            var film = await _context.Films
                .Include(f => f.FilmActors)
                .Include(f => f.FilmCategories)
                .FirstOrDefaultAsync(f => f.FilmId == model.FilmId);

            if (film == null) return NotFound();

            // Actualiza campos base
            film.Title = model.Title;
            film.Description = model.Description;
            film.ReleaseYear = model.ReleaseYear;
            film.LanguageId = model.LanguageId;
            film.OriginalLanguageId = model.OriginalLanguageId;
            film.RentalDuration = model.RentalDuration;
            film.RentalRate = model.RentalRate;
            film.Length = model.Length;
            film.ReplacementCost = model.ReplacementCost;
            film.Rating = model.Rating;
            film.LastUpdate = DateTime.UtcNow;

            // Actualiza actores
            _context.FilmActors.RemoveRange(film.FilmActors);
            foreach (var actorId in model.ActoresSeleccionados)
            {
                film.FilmActors.Add(new FilmActor
                {
                    ActorId = actorId,
                    FilmId = film.FilmId,
                    LastUpdate = DateTime.UtcNow
                });
            }

            // Actualiza categorías
            _context.FilmCategories.RemoveRange(film.FilmCategories);
            foreach (var catId in model.CategoriasSeleccionadas)
            {
                film.FilmCategories.Add(new FilmCategory
                {
                    CategoryId = catId,
                    FilmId = film.FilmId,
                    LastUpdate = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Película actualizada correctamente." });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ushort id)
        {
            try
            {
                var film = await _context.Films.FindAsync(id);
                if (film == null) return NotFound();

                _context.Films.Remove(film);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Película eliminada correctamente." });
            }
            catch
            {
                return Json(new { success = false, message = "No se puede eliminar la película, tiene relaciones asociadas." });
            }
        }

        // Helpers
        private async Task CargarListas(FilmViewModel model)
        {
            model.Idiomas = await GetIdiomasAsync();
            model.IdiomasOriginales = await GetIdiomasAsync(true);
            model.ActoresDisponibles = await GetActoresAsync();
            model.CategoriasDisponibles = await GetCategoriasAsync();
        }

        private async Task<List<SelectListItem>> GetIdiomasAsync(bool incluirNulo = false)
        {
            var idiomas = await _context.Languages
                .OrderBy(l => l.Name)
                .Select(l => new SelectListItem
                {
                    Value = l.LanguageId.ToString(),
                    Text = l.Name
                }).ToListAsync();

            if (incluirNulo)
                idiomas.Insert(0, new SelectListItem { Text = "-- Ninguno --", Value = "" });

            return idiomas;
        }

        private async Task<List<SelectListItem>> GetActoresAsync()
        {
            return await _context.Actors
                .OrderBy(a => a.FirstName)
                .Select(a => new SelectListItem
                {
                    Value = a.ActorId.ToString(),
                    Text = a.FirstName + " " + a.LastName
                }).ToListAsync();
        }

        private async Task<List<SelectListItem>> GetCategoriasAsync()
        {
            return await _context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                }).ToListAsync();
        }
    }
}
