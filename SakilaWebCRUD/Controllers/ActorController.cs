using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SakilaWebCRUD.Datos;
using SakilaWebCRUD.Models;
using SakilaWebCRUD.Models.ViewModels;

namespace SakilaWebCRUD.Controllers
{
    public class ActorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var actores = await _context.Actors
                .Select(a => new
                {
                    a.ActorId,
                    a.FirstName,
                    a.LastName,
                    LastUpdate = a.LastUpdate.ToString("yyyy-MM-dd HH:mm:ss")
                }).ToListAsync();

            return Json(new { data = actores });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_CreateEditModal", new ActorViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ActorViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("_CreateEditModal", model);

            _context.Actors.Add(new Actor
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                LastUpdate = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Actor creado correctamente." });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(ushort id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null) return NotFound();

            var model = new ActorViewModel
            {
                ActorId = actor.ActorId,
                FirstName = actor.FirstName,
                LastName = actor.LastName,
                LastUpdate = actor.LastUpdate
            };

            return PartialView("_CreateEditModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ActorViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("_CreateEditModal", model);

            var actor = await _context.Actors.FindAsync(model.ActorId);
            if (actor == null) return NotFound();

            actor.FirstName = model.FirstName;
            actor.LastName = model.LastName;
            actor.LastUpdate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Actor actualizado correctamente." });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ushort id)
        {
            try
            {
                var actor = await _context.Actors.FindAsync(id);
                if (actor == null) return NotFound();

                _context.Actors.Remove(actor);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Actor eliminado correctamente." });
            }
            catch
            {
                return Json(new { success = false, message = "No se pudo eliminar. El actor tiene relaciones asociadas." });
            }
        }
    }
}
