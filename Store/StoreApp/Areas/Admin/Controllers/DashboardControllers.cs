using Microsoft.AspNetCore.Mvc;

namespace StoreApp.Areas.Admin.Controllers.DashboardControllers.cs
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}