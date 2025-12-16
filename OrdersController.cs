using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebHomework.Data;
using WebHomework.Models;

namespace WebHomework.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // =====================================================
        // GET: Orders  (LİSTELEME)
        // =====================================================
        public async Task<IActionResult> Index()
        {
            // Admin tüm siparişleri görür
            if (User.IsInRole("Admin"))
            {
                return View(await _context.Orders
                    .Include(o => o.User)        // 🔥 ÖNEMLİ
                    .Include(o => o.GymService)
                    .Include(o => o.Trainer)
                    .ToListAsync());
            }

            // Normal kullanıcı sadece kendi siparişlerini görür
            var userId = _userManager.GetUserId(User);

            return View(await _context.Orders
                .Include(o => o.User)            // 🔥 ÖNEMLİ
                .Include(o => o.GymService)
                .Include(o => o.Trainer)
                .Where(o => o.UserId == userId)
                .ToListAsync());
        }

        // =====================================================
        // GET: Orders/Create
        // =====================================================
        public IActionResult Create()
        {
            // Kullanıcılar
            var userList = _context.Users
                .Select(u => new { u.Id, u.Email })
                .ToList();

            ViewData["UserId"] = new SelectList(userList, "Id", "Email");

            // Hizmetler  ✅ (EKSİK OLAN BUYDU)
            ViewData["GymServiceId"] = new SelectList(
                _context.GymServices,
                "Id",
                "Name"
            );

            // Eğitmenler
            ViewData["TrainerId"] = new SelectList(
                _context.Trainers,
                "Id",
                "FullName"
            );

            return View();
        }

        // =====================================================
        // POST: Orders/Create
        // =====================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("GymServiceId,UserId,TrainerId,OrderDate")] Order order)
        {
            // 1️⃣ Fiyatı hizmetten otomatik al
            var service = await _context.GymServices
                .FindAsync(order.GymServiceId);

            if (service != null)
            {
                order.Price = service.Price;
            }

            // 2️⃣ Eğitmen saat çakışma kontrolü
            if (order.TrainerId != null)
            {
                bool isBusy = _context.Orders.Any(o =>
                    o.TrainerId == order.TrainerId &&
                    o.OrderDate == order.OrderDate);

                if (isBusy)
                {
                    ModelState.AddModelError(
                        "OrderDate",
                        "Bu eğitmen seçilen tarih ve saatte dolu."
                    );
                }
            }

            // 3️⃣ Navigation alanlarını temizle
            ModelState.Remove("Price");
            ModelState.Remove("User");
            ModelState.Remove("GymService");
            ModelState.Remove("Trainer");

            // 4️⃣ Kaydet
            if (ModelState.IsValid)
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // 5️⃣ Hata varsa dropdown’ları yeniden doldur
            var userList = _context.Users
                .Select(u => new { u.Id, u.Email })
                .ToList();

            ViewData["UserId"] = new SelectList(userList, "Id", "Email", order.UserId);
            ViewData["GymServiceId"] = new SelectList(_context.GymServices, "Id", "Name", order.GymServiceId);
            ViewData["TrainerId"] = new SelectList(_context.Trainers, "Id", "FullName", order.TrainerId);
            // "Name" yerine eğer modelinizde "ServiceName" veya "Title" varsa onu yazın!
            ViewData["GymServiceId"] = new SelectList(_context.GymServices, "Id", "Name");
            return View(order);
        }

        // =====================================================
        // GET: Orders/Delete
        // =====================================================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.GymService)
                .Include(o => o.Trainer)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            // Güvenlik
            if (!User.IsInRole("Admin") &&
                order.UserId != _userManager.GetUserId(User))
                return Forbid();

            return View(order);
        }

        // =====================================================
        // POST: Orders/Delete
        // =====================================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order != null)
            {
                if (!User.IsInRole("Admin") &&
                    order.UserId != _userManager.GetUserId(User))
                    return Forbid();

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

