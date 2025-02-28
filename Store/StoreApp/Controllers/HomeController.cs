using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Services.Abstract;
using Entities.Models;

namespace StoreApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServiceManager _manager;

        public HomeController(IServiceManager manager)
        {
            _manager = manager;
        }

        public async Task<IActionResult> Index()
        {
            // Son eklenen 3 ürünü ViewBag'e ekle
            var products = await _manager.ProductService.GetAllProductsAsync();
            ViewBag.LatestProducts = products
                .OrderByDescending(p => p.ProductId)
                .Take(3)
                .ToList();

            // Tüm ürünleri Model olarak gönder
            return View(products);
        }
        
    }
}

