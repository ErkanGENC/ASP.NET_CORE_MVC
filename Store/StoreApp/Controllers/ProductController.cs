using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;

namespace StoreApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IRepositoryManager _manager;

        public ProductController(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public IActionResult Index()
        {
            var products = _manager.Product.GetAllProducts(false).ToList();
            return View(products);
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