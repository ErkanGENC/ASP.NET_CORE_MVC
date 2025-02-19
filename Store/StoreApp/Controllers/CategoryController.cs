using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;

namespace StoreApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IRepositoryManager _manager;

        public CategoryController(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public IActionResult Index()
        {
            var categories = _manager.Category.GetAllCategories(false).ToList();
            return View(categories);
        }

        public IActionResult Get(int id)
        {
            var category = _manager.Category.GetOneCategoryById(id, false);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
    }
} 