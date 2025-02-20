using Microsoft.AspNetCore.Mvc;

namespace StoreApp.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string name, string email, string subject, string message)
        {
            // Burada form verilerini işleyebilirsiniz
            // Örneğin: E-posta gönderme, veritabanına kaydetme vb.

            // Şimdilik sadece TempData ile bir mesaj gösterelim
            TempData["Message"] = "Mesajınız başarıyla gönderildi. En kısa sürede size dönüş yapacağız.";
            
            return RedirectToAction("Index");
        }
    }
} 