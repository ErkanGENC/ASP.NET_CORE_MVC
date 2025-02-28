using Microsoft.AspNetCore.Mvc;
using Services.Abstract;
using StoreApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StoreApp.Components
{
    public class ProductFilterMenuViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;

        public ProductFilterMenuViewComponent(IServiceManager manager)
        {
            _manager = manager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _manager.CategoryService.GetAllCategoriesAsync();
            
            var viewModel = new ProductFilterViewModel
            {
                Categories = new SelectList(categories, "CategoryId", "CategoryName"),
                SortOptions = new Dictionary<string, string>
                {
                    { "name_asc", "İsim (A-Z)" },
                    { "name_desc", "İsim (Z-A)" },
                    { "price_asc", "Fiyat (Düşük-Yüksek)" },
                    { "price_desc", "Fiyat (Yüksek-Düşük)" }
                }
            };

            return View(viewModel);
        }
    }
} 