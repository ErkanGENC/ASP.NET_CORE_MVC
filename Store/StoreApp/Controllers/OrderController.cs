using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using StoreApp.Models;
using Entities.Models;

namespace StoreApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IServiceManager _manager;
        private readonly Cart _cart;

        public OrderController(IServiceManager manager, Cart cartService)
        {
            _manager = manager;
            _cart = cartService;
        }

        public IActionResult Checkout()
        {
            if (_cart.Lines.Count() == 0)
            {
                TempData["Message"] = "Sepetiniz boş!";
                return RedirectToPage("/Cart/Cart");
            }

            ViewBag.Cart = _cart;
            return View(new Order());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout([FromForm] Order order)
        {
            if (_cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sepetiniz boş!");
            }

            if (ModelState.IsValid)
            {
                try 
                {
                    // CartLine'ları OrderLine'lara dönüştür
                    order.Lines = new List<OrderLine>();
                    foreach (var line in _cart.Lines)
                    {
                        var product = await _manager.ProductService.GetProductByIdAsync(line.Product.Id);
                        order.Lines.Add(new OrderLine
                        {
                            ProductId = line.Product.Id,
                            Quantity = line.Quantity,
                            Price = line.Product.Price
                        });
                    }

                    order.OrderTotal = _cart.ComputeTotalValue() + (order.GiftWrap ? 20 : 0) + 29.90m; // Kargo ücreti
                    order.OrderDate = DateTime.Now;
                    order.OrderStatus = "Beklemede";

                    await _manager.OrderService.CreateOrderAsync(order);

                    _cart.Clear();

                    TempData["Message"] = "Siparişiniz başarıyla alındı!";
                    return RedirectToAction("Completed", new { id = order.OrderId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Sipariş oluşturulurken bir hata oluştu: " + ex.Message);
                }
            }

            ViewBag.Cart = _cart;
            return View(order);
        }

        public async Task<IActionResult> Completed(int id)
        {
            var order = await _manager.OrderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            return View(order);
        }
    }
} 