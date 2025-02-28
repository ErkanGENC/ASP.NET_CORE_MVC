using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using Entities.Models;

namespace StoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IServiceManager _manager;

        public OrderController(IServiceManager manager)
        {
            _manager = manager;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _manager.OrderService.GetAllOrdersAsync();
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _manager.OrderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            try
            {
                var order = await _manager.OrderService.GetOrderByIdAsync(id);
                if (order == null)
                    return NotFound();

                order.OrderStatus = status;
                await _manager.OrderService.UpdateOrderAsync(order);

                TempData["Message"] = "Sipariş durumu başarıyla güncellendi.";
                return RedirectToAction(nameof(Details), new { id = id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata: {ex.Message}";
                return RedirectToAction(nameof(Details), new { id = id });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _manager.OrderService.DeleteOrderAsync(id);
                TempData["Message"] = "Sipariş başarıyla silindi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hata: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
} 