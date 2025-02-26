using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using Entities.Models;
using Entities.Dtos;
using System.Linq;

namespace StoreApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IServiceManager _manager;

        public ProductController(IServiceManager manager)
        {
            _manager = manager;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            var products = await _manager.ProductService.GetAllProductsAsync();
            
            if (categoryId.HasValue)
            {
                var category = await _manager.CategoryService.GetCategoryByIdAsync(categoryId.Value);
                if (category != null)
                {
                    ViewBag.SelectedCategory = category;
                    products = products.Where(p => p.CategoryId == categoryId.Value);
                }
            }

            return View(products.ToList());
        }

        public async Task<IActionResult> Get(int id)
        {
            var product = await _manager.ProductService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();  
            }
            return View(product);
        }
    }
}