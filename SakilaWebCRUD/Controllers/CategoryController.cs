using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SakilaWebCRUD.Datos;
using SakilaWebCRUD.Models;
using SakilaWebCRUD.Models.ViewModels;

namespace SakilaWebCRUD.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
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
            try
            {
                var categorias = await _context.Categories
                    .Select(c => new
                    {
                        c.CategoryId,
                        c.Name,
                        LastUpdate = c.LastUpdate.ToString("yyyy-MM-dd HH:mm:ss")
                    }).ToListAsync();

                return Json(new { data = categorias });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener las categorías.", detail = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_CreateEditModal", new CategoryViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var categoria = new Category
                    {
                        Name = model.Name,
                        LastUpdate = DateTime.UtcNow
                    };

                    _context.Categories.Add(categoria);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Categoría creada correctamente." });
                }

                return PartialView("_CreateEditModal", model);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al crear la categoría.", detail = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(byte id)
        {
            var categoria = await _context.Categories.FindAsync(id);
            if (categoria == null) return NotFound();

            var model = new CategoryViewModel
            {
                CategoryId = categoria.CategoryId,
                Name = categoria.Name,
                LastUpdate = categoria.LastUpdate
            };

            return PartialView("_CreateEditModal", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var categoria = await _context.Categories.FindAsync(model.CategoryId);
                    if (categoria == null) return NotFound();

                    categoria.Name = model.Name;
                    categoria.LastUpdate = DateTime.UtcNow;

                    _context.Update(categoria);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Categoría actualizada correctamente." });
                }

                return PartialView("_CreateEditModal", model);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al editar la categoría.", detail = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(byte id)
        {
            try
            {
                var categoria = await _context.Categories.FindAsync(id);
                if (categoria == null) return NotFound();

                _context.Categories.Remove(categoria);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Categoría eliminada correctamente." });
            }
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException mysqlEx && mysqlEx.Number == 1451)
            {
                return Json(new { success = false, message = "No se puede eliminar esta categoría porque está en uso." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error inesperado al eliminar.", detail = ex.Message });
            }
        }
    }
}
