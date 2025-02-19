using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Services.Abstract;

namespace StoreApp.Components
{
    public class CategoryCountViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;

        public CategoryCountViewComponent(IServiceManager manager)
        {
            _manager = manager;
        }

        public string Invoke()
        {
            return _manager.CategoryService.GetAllCategoriesAsync().Result.Count().ToString();
        }
    }
} 