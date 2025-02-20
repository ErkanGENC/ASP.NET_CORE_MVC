using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstract;
using StoreApp.Infrastructure.Extensions;
using StoreApp.Models;
using Entities.Models;
using Entities.Dtos;

namespace StoreApp.Pages.Cart
{
    public class CartModel : PageModel
    {
        private readonly IServiceManager _manager;
        private readonly Models.Cart _cart;

        public Models.Cart Cart => _cart;

        [TempData]
        public string Message { get; set; }

        public CartModel(IServiceManager manager, Models.Cart cartService)
        {
            _manager = manager;
            _cart = cartService;
        }

        public void OnGet()
        {
            // Sayfa yüklendiğinde yapılacak işlemler
        }

        private Product ConvertToProduct(ProductDto dto)
        {
            return new Product
            {
                Id = dto.Id,
                Name = dto.Name,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                Description = dto.Description,
                CategoryId = dto.CategoryId
            };
        }

        public IActionResult OnPostAdd(int id, int quantity = 1, string customerName = "", 
            string email = "", string phone = "", string address = "", string city = "", string postalCode = "")
        {
            var productDto = _manager.ProductService.GetProductByIdAsync(id).Result;
            if (productDto != null)
            {
                var product = ConvertToProduct(productDto);
                Cart.AddItem(product, quantity);

                // Müşteri bilgilerini TempData'ya kaydedelim
                TempData["CustomerName"] = customerName;
                TempData["Email"] = email;
                TempData["Phone"] = phone;
                TempData["Address"] = address;
                TempData["City"] = city;
                TempData["PostalCode"] = postalCode;

                Message = "Ürün başarıyla sepete eklendi.";
            }
            return RedirectToPage();
        }

        public IActionResult OnPostRemove(int id)
        {
            var productDto = _manager.ProductService.GetProductByIdAsync(id).Result;
            if (productDto != null)
            {
                var product = ConvertToProduct(productDto);
                Cart.RemoveLine(product);
                Message = "Ürün sepetten kaldırıldı.";
            }
            return RedirectToPage();
        }

        public IActionResult OnPostUpdateQuantity(int id, int quantity)
        {
            var productDto = _manager.ProductService.GetProductByIdAsync(id).Result;
            if (productDto != null)
            {
                var product = ConvertToProduct(productDto);
                Cart.UpdateQuantity(product, quantity);
                Message = "Ürün miktarı güncellendi.";
            }
            return RedirectToPage();
        }

        public IActionResult OnPostClear()
        {
            Cart.Clear();
            Message = "Sepet temizlendi.";
            return RedirectToPage();
        }
    }
} 