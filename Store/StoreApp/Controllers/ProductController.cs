using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using System.Linq;
using Entities.Models;

namespace StoreApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IRepositoryManager _manager;

        public ProductController(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public IActionResult Index(int? categoryId)
        {
            var products = _manager.Product.GetAllProducts(false);
            
            if (categoryId.HasValue)
            {
                var category = _manager.Category.GetOneCategoryById(categoryId.Value, false);
                if (category != null)
                {
                    ViewBag.SelectedCategory = category;
                    // Burada kategori filtrelemesi yapılacak (ilişki kurulduktan sonra)
                    // products = products.Where(p => p.CategoryId == categoryId.Value);
                }
            }

            return View(products.ToList());
        }

        public IActionResult Get(int id)
        {
            var product = _manager.Product.GetOneProduct(id, false);
            if (product == null)
            {
                return NotFound();  
            }
            return View(product);
        }
    }
}