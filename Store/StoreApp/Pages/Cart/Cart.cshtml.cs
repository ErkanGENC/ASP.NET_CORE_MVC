using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstract;
using StoreApp.Infrastructure.Extensions;
using StoreApp.Models;
using Entities.Models;

namespace StoreApp.Pages
{
    public class CartModel : PageModel
    {
        private readonly IServiceManager _manager;
        private readonly Cart _cart;

        [TempData]
        public string? Message { get; set; }

        public CartModel(IServiceManager manager, Cart cartService)
        {
            _manager = manager;
            _cart = cartService;
        }

        public Cart Cart => _cart;
        public string ReturnUrl { get; set; } = "/";

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
        }

        public async Task<IActionResult> OnPostAddAsync(int id, string returnUrl)
        {
            try
            {
                var product = await _manager.ProductService.GetProductByIdAsync(id);
                if (product != null)
                {
                    var productModel = new Product
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        ImageUrl = product.ImageUrl,
                        Description = product.Description
                    };

                    _cart.AddItem(productModel, 1);
                    Message = $"{product.Name} başarıyla sepete eklendi.";
                }
                else
                {
                    Message = "Ürün bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                Message = $"Hata: {ex.Message}";
            }
            return RedirectToPage(new { returnUrl = returnUrl });
        }

        public IActionResult OnPostRemove(int id)
        {
            var product = _cart.Lines.FirstOrDefault(cl => cl.Product.Id == id)?.Product;
            if (product != null)
            {
                _cart.RemoveLine(product);
                Message = "Ürün sepetten kaldırıldı.";
            }
            return RedirectToPage();
        }

        public IActionResult OnPostClear()
        {
            _cart.Clear();
            Message = "Sepet temizlendi.";
            return RedirectToPage();
        }

        public IActionResult OnPostUpdateQuantity(int id, int quantity)
        {
            try
            {
                if (quantity < 1)
                {
                    quantity = 1;
                }
                else if (quantity > 100)
                {
                    quantity = 100;
                }

                var product = _cart.Lines.FirstOrDefault(cl => cl.Product.Id == id)?.Product;
                if (product != null)
                {
                    _cart.UpdateQuantity(product, quantity);
                    Message = "Ürün miktarı güncellendi.";
                }
                else
                {
                    Message = "Ürün bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                Message = $"Hata: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
} 