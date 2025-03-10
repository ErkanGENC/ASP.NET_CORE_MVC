using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using StoreApp.Models;
using Entities.Models;
using Entities.Dtos;

namespace StoreApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IServiceManager _manager;
        private readonly Cart _cart;

        public CartController(IServiceManager manager, Cart cart)
        {
            _manager = manager;
            _cart = cart;
        }

        public IActionResult Index()
        {
            return View(_cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromForm] int productId, [FromForm] int quantity = 1)
        {
            var productDto = await _manager.ProductService.GetProductByIdAsync(productId);
            if (productDto == null)
            {
                TempData["Error"] = "Ürün bulunamadı.";
                return RedirectToAction("Index", "Home");
            }

            var product = new Product
            {
                ProductId = productDto.ProductId,
                Name = productDto.Name,
                Price = productDto.Price,
                ImageUrl = productDto.ImageUrl,
                Description = productDto.Description,
                CategoryId = productDto.CategoryId ?? 0
            };

            _cart.AddItem(product, quantity);
            TempData["Success"] = $"{product.Name} sepete eklendi.";
            
            // AJAX isteği ise JSON döndür
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { 
                    success = true, 
                    message = $"{product.Name} sepete eklendi.", 
                    cartItemCount = _cart.TotalItems() 
                });
            }
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetCartItemCount()
        {
            return Json(new { count = _cart.TotalItems() });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var productDto = await _manager.ProductService.GetProductByIdAsync(productId);
            if (productDto != null)
            {
                var product = new Product
                {
                    ProductId = productDto.ProductId,
                    Name = productDto.Name,
                    Price = productDto.Price,
                    ImageUrl = productDto.ImageUrl,
                    Description = productDto.Description,
                    CategoryId = productDto.CategoryId ?? 0
                };
                _cart.RemoveLine(product);
                TempData["Success"] = $"{product.Name} sepetten çıkarıldı.";
                
                // AJAX isteği ise JSON döndür
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { 
                        success = true, 
                        message = $"{product.Name} sepetten çıkarıldı.", 
                        cartItemCount = _cart.TotalItems() 
                    });
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            var productDto = await _manager.ProductService.GetProductByIdAsync(productId);
            if (productDto != null)
            {
                var product = new Product
                {
                    ProductId = productDto.ProductId,
                    Name = productDto.Name,
                    Price = productDto.Price,
                    ImageUrl = productDto.ImageUrl,
                    Description = productDto.Description,
                    CategoryId = productDto.CategoryId ?? 0
                };
                _cart.UpdateQuantity(product, quantity);
                TempData["Success"] = "Sepet güncellendi.";
                
                // AJAX isteği ise JSON döndür
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { 
                        success = true, 
                        message = "Sepet güncellendi.", 
                        cartItemCount = _cart.TotalItems() 
                    });
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            _cart.Clear();
            TempData["Success"] = "Sepet temizlendi.";
            
            // AJAX isteği ise JSON döndür
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { 
                    success = true, 
                    message = "Sepet temizlendi.", 
                    cartItemCount = 0 
                });
            }
            
            return RedirectToAction("Index");
        }
    }
} 