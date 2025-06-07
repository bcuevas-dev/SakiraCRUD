using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SakilaWebCRUD.Datos;
using SakilaWebCRUD.Models;
using SakilaWebCRUD.Models.ViewModels;

namespace SakilaWebCRUD.Controllers
{
    public class InventoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoryController(ApplicationDbContext context)
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
            var data = await _context.Inventories
                .Include(i => i.Film)
                .Include(i => i.Store)
                .Select(i => new
                {
                    i.InventoryId,
                    FilmTitle = i.Film.Title,
                    StoreId = i.Store.StoreId,
                    LastUpdate = i.LastUpdate.ToString("yyyy-MM-dd HH:mm:ss")
                })
                .ToListAsync();

            return Json(new { data });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new InventoryViewModel
            {
                Peliculas = await GetPeliculasAsync(),
                Tiendas = await GetTiendasAsync()
            };
            return PartialView("_CreateEditModal", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(InventoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var inventory = new Inventory
                {
                    FilmId = vm.FilmId,
                    StoreId = vm.StoreId,
                    LastUpdate = DateTime.UtcNow
                };

                _context.Inventories.Add(inventory);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Inventario creado correctamente." });
            }

            vm.Peliculas = await GetPeliculasAsync();
            vm.Tiendas = await GetTiendasAsync();
            return PartialView("_CreateEditModal", vm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(uint id)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null) return NotFound();

            var vm = new InventoryViewModel
            {
                InventoryId = inventory.InventoryId,
                FilmId = inventory.FilmId,
                StoreId = inventory.StoreId,
                LastUpdate = inventory.LastUpdate,
                Peliculas = await GetPeliculasAsync(),
                Tiendas = await GetTiendasAsync()
            };

            return PartialView("_CreateEditModal", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(InventoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var inventory = await _context.Inventories.FindAsync(vm.InventoryId);
                if (inventory == null) return NotFound();

                inventory.FilmId = vm.FilmId;
                inventory.StoreId = vm.StoreId;
                inventory.LastUpdate = DateTime.UtcNow;

                _context.Update(inventory);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Inventario actualizado correctamente." });
            }

            vm.Peliculas = await GetPeliculasAsync();
            vm.Tiendas = await GetTiendasAsync();
            return PartialView("_CreateEditModal", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(uint id)
        {
            try
            {
                var inventory = await _context.Inventories.FindAsync(id);
                if (inventory == null) return NotFound();

                _context.Inventories.Remove(inventory);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Inventario eliminado correctamente." });
            }
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException sqlEx && sqlEx.Number == 1451)
            {
                return Json(new { success = false, message = "No se puede eliminar: tiene datos relacionados." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error inesperado al eliminar.", detail = ex.Message });
            }
        }

        // Helpers para dropdowns
        private async Task<List<SelectListItem>> GetPeliculasAsync()
        {
            return await _context.Films
                .OrderBy(f => f.Title)
                .Select(f => new SelectListItem
                {
                    Value = f.FilmId.ToString(),
                    Text = f.Title
                }).ToListAsync();
        }

        private async Task<List<SelectListItem>> GetTiendasAsync()
        {
            return await _context.Stores
                .OrderBy(s => s.StoreId)
                .Select(s => new SelectListItem
                {
                    Value = s.StoreId.ToString(),
                    Text = "Tienda #" + s.StoreId
                }).ToListAsync();
        }
    }
}
