using Microsoft.AspNetCore.Mvc;
using Services.Abstract;

namespace StoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}