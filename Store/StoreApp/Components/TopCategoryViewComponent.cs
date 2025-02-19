using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;

namespace StoreApp.Components
{
    public class TopCategoryViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;

        public TopCategoryViewComponent(IServiceManager manager)
        {
            _manager = manager;
        }

        public string Invoke()
        {
            var categories = _manager.CategoryService.GetAllCategoriesAsync().Result;
            if (!categories.Any())
                return "-";

            // Şimdilik sadece ilk kategoriyi döndürüyoruz
            // İleride ürün-kategori ilişkisi kurulduğunda en çok ürünü olan kategori gösterilecek
            return categories.First().CategoryName ?? "-";
        }
    }
} 