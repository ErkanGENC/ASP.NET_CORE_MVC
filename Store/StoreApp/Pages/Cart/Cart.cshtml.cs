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

        public async Task<IActionResult> OnPostAddAsync(int id, int quantity, string returnUrl)
        {
            try
            {
                var product = await _manager.ProductService.GetProductByIdAsync(id);
                if (product != null)
                {
                    _cart.AddItem(new Product
                    {
                        ProductId = product.ProductId,
                        Name = product.Name,
                        Price = product.Price,
                        ImageUrl = product.ImageUrl
                    }, quantity);

                    Message = $"{product.Name} sepete eklendi.";
                }
                return RedirectToPage(new { returnUrl = returnUrl });
            }
            catch (Exception ex)
            {
                Message = $"Hata: {ex.Message}";
                return RedirectToPage(new { returnUrl = returnUrl });
            }
        }

        public IActionResult OnPostRemove(int id)
        {
            var product = _cart.Lines.FirstOrDefault(cl => cl.Product.ProductId == id)?.Product;
            if (product != null)
            {
                _cart.RemoveLine(product);
                Message = $"{product.Name} sepetten çıkarıldı.";
            }
            return RedirectToPage();
        }

        public IActionResult OnPostClear()
        {
            _cart.Clear();
            Message = "Sepetiniz temizlendi.";
            return RedirectToPage();
        }

        public IActionResult OnPostUpdateQuantity(int id, int quantity)
        {
            var product = _cart.Lines.FirstOrDefault(cl => cl.Product.ProductId == id)?.Product;
            if (product != null)
            {
                if (quantity > 0)
                {
                    _cart.UpdateQuantity(product, quantity);
                    Message = $"{product.Name} ürününün adedi güncellendi.";
                }
                else
                {
                    _cart.RemoveLine(product);
                    Message = $"{product.Name} sepetten çıkarıldı.";
                }
            }
            return RedirectToPage();
        }
    }
} 