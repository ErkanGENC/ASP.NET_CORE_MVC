using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using Entities.Models;
using Entities.Dtos;
using StoreApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repositories.Contracts;

namespace StoreApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IServiceManager _manager;
        private const int PageSize = 9; // Her sayfada 9 ürün gösterilecek

        public ProductController(IServiceManager manager)
        {
            _manager = manager;
        }

        public async Task<IActionResult> Index(ProductFilterViewModel filter, int page = 1)
        {
            var products = (await _manager.ProductService.GetAllProductsAsync()).ToList();
            var categories = await _manager.CategoryService.GetAllCategoriesAsync();

            // Filtreleme işlemleri
            var filteredProducts = products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                filteredProducts = filteredProducts.Where(p => 
                    p.Name != null && p.Name.Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (filter.MinPrice.HasValue)
            {
                filteredProducts = filteredProducts.Where(p => p.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                filteredProducts = filteredProducts.Where(p => p.Price <= filter.MaxPrice.Value);
            }

            if (filter.CategoryId.HasValue)
            {
                filteredProducts = filteredProducts.Where(p => p.CategoryId == filter.CategoryId.Value);
            }

            // Sıralama işlemleri
            filteredProducts = filter.SortBy switch
            {
                "name_asc" => filteredProducts.OrderBy(p => p.Name),
                "name_desc" => filteredProducts.OrderByDescending(p => p.Name),
                "price_asc" => filteredProducts.OrderBy(p => p.Price),
                "price_desc" => filteredProducts.OrderByDescending(p => p.Price),
                _ => filteredProducts
            };

            // Sayfalama işlemleri
            var totalItems = filteredProducts.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            
            var pagedProducts = filteredProducts
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var viewModel = new ProductFilterViewModel
            {
                Products = pagedProducts,
                SearchTerm = filter.SearchTerm,
                MinPrice = filter.MinPrice,
                MaxPrice = filter.MaxPrice,
                CategoryId = filter.CategoryId,
                Categories = new SelectList(categories, "CategoryId", "CategoryName"),
                SortBy = filter.SortBy,
                CurrentPage = page,
                TotalPages = totalPages
            };

            // Son eklenen ürünleri ViewBag'e ekle
            ViewBag.LatestProducts = products
                .OrderByDescending(p => p.ProductId)
                .Take(3)
                .ToList();

            return View(viewModel);
        }

        public async Task<IActionResult> Get(int id)
        {
            try 
            {
                var product = await _manager.ProductService.GetProductByIdAsync(id);
                return View(product);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}