using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SakilaWebCRUD.Datos;
using SakilaWebCRUD.Models;
using SakilaWebCRUD.ViewModels;

namespace SakilaWebCRUD.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
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
                var payments = await _context.Payments
                    .Include(p => p.Customer)
                    .Include(p => p.Staff)
                    .Include(p => p.Rental)
                    .Select(p => new
                    {
                        p.PaymentId,
                        CustomerName = p.Customer.FirstName + " " + p.Customer.LastName,
                        StaffName = p.Staff.FirstName + " " + p.Staff.LastName,
                        RentalInfo = p.Rental != null ? p.Rental.RentalDate.ToString("yyyy-MM-dd HH:mm") : "N/A",
                        p.Amount,
                        PaymentDate = p.PaymentDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        LastUpdate = p.LastUpdate.HasValue ? p.LastUpdate.Value.ToString("yyyy-MM-dd HH:mm:ss") : ""
                    })
                    .ToListAsync();

                return Json(new { data = payments });
            }
            catch
            {
                return BadRequest("Error al obtener los pagos.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new PaymentViewModel
            {
                Clientes = await _context.Customers
                    .Select(c => new SelectListItem
                    {
                        Value = c.CustomerId.ToString(),
                        Text = c.FirstName + " " + c.LastName
                    }).ToListAsync(),

                Empleados = await _context.Staff
                    .Select(s => new SelectListItem
                    {
                        Value = s.StaffId.ToString(),
                        Text = s.FirstName + " " + s.LastName
                    }).ToListAsync(),

                Rentas = await _context.Rentals
                    .Select(r => new SelectListItem
                    {
                        Value = r.RentalId.ToString(),
                        Text = r.RentalDate.ToString("yyyy-MM-dd HH:mm")
                    }).ToListAsync()
            };

            return PartialView("_CreateEditModal", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaymentViewModel vm)
        {
            if (!ModelState.IsValid)
                return PartialView("_CreateEditModal", vm);

            var payment = new Payment
            {
                CustomerId = vm.CustomerId,
                StaffId = vm.StaffId,
                RentalId = vm.RentalId,
                Amount = vm.Amount,
                PaymentDate = vm.PaymentDate,
                LastUpdate = DateTime.Now
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Pago registrado exitosamente." });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(ushort id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return NotFound();

            var vm = new PaymentViewModel
            {
                PaymentId = payment.PaymentId,
                CustomerId = payment.CustomerId,
                StaffId = payment.StaffId,
                RentalId = payment.RentalId,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                LastUpdate = payment.LastUpdate,

                Clientes = await _context.Customers
                    .Select(c => new SelectListItem
                    {
                        Value = c.CustomerId.ToString(),
                        Text = c.FirstName + " " + c.LastName
                    }).ToListAsync(),

                Empleados = await _context.Staff
                    .Select(s => new SelectListItem
                    {
                        Value = s.StaffId.ToString(),
                        Text = s.FirstName + " " + s.LastName
                    }).ToListAsync(),

                Rentas = await _context.Rentals
                    .Select(r => new SelectListItem
                    {
                        Value = r.RentalId.ToString(),
                        Text = r.RentalDate.ToString("yyyy-MM-dd HH:mm")
                    }).ToListAsync()
            };

            return PartialView("_CreateEditModal", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PaymentViewModel vm)
        {
            if (!ModelState.IsValid)
                return PartialView("_CreateEditModal", vm);

            var payment = await _context.Payments.FindAsync(vm.PaymentId);
            if (payment == null) return NotFound();

            payment.CustomerId = vm.CustomerId;
            payment.StaffId = vm.StaffId;
            payment.RentalId = vm.RentalId;
            payment.Amount = vm.Amount;
            payment.PaymentDate = vm.PaymentDate;
            payment.LastUpdate = DateTime.Now;

            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Pago actualizado correctamente." });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ushort id)
        {
            try
            {
                var payment = await _context.Payments.FindAsync(id);
                if (payment == null) return Json(new { success = false, message = "Pago no encontrado." });

                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Pago eliminado correctamente." });
            }
            catch (DbUpdateException ex)
            {
                return Json(new { success = false, message = "Error al eliminar: el registro está relacionado con otros datos." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error inesperado al eliminar el pago." });
            }
        }
    }
}
