using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;

namespace StoreApp.Components
{
    public class CategoryProductCountViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;

        public CategoryProductCountViewComponent(IServiceManager manager)
        {
            _manager = manager;
        }

        public string Invoke(int categoryId)
        {
            // Şimdilik sabit bir değer döndürüyoruz
            // İleride ürün-kategori ilişkisi kurulduğunda gerçek sayı hesaplanacak
            return "0";
        }
    }
} 