using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace StoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            Debug.WriteLine("DashboardController.Index çağrıldı");
            Debug.WriteLine($"Kullanıcı yetkilendirmesi: {User.Identity?.IsAuthenticated}");
            
            // Oturum açan kullanıcının rollerini yazdır
            if (User.Identity.IsAuthenticated)
            {
                Debug.WriteLine($"Kullanıcı: {User.Identity.Name}");
                Debug.WriteLine($"Admin mi: {User.IsInRole("Admin")}");
                foreach (var claim in User.Claims)
                {
                    Debug.WriteLine($"Claim: {claim.Type} - {claim.Value}");
                }
            }
            
            return View();
        }
    }
} 