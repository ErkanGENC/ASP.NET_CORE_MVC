using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using StoreApp.Models.ViewModels;
using System.Diagnostics;

namespace StoreApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    Debug.WriteLine($"Login attempt for email: {model.Email}");
                    
                    // Email ile kullanıcıyı bul
                    IdentityUser user = await _userManager.FindByEmailAsync(model.Email);
                    if(user != null)
                    {
                        Debug.WriteLine($"User found: {user.UserName}, Email: {user.Email}");
                        
                        await _signInManager.SignOutAsync();
                        
                        // Kullanıcı adı ile giriş yap
                        var result = await _signInManager.PasswordSignInAsync(
                            userName: user.UserName,
                            password: model.Password, 
                            isPersistent: model.RememberMe, 
                            lockoutOnFailure: true);
                            
                        Debug.WriteLine($"Login result: {result.Succeeded}");
                            
                        if(result.Succeeded)
                        {
                            // Admin rolünü kontrol et
                            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                            Debug.WriteLine($"Is admin: {isAdmin}");
                            
                            if (isAdmin)
                            {
                                Debug.WriteLine("Redirecting to admin dashboard");
                                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                            }
                            
                            Debug.WriteLine($"Redirecting to: {model.ReturnUrl ?? "/"}");
                            return LocalRedirect(model.ReturnUrl ?? "/");
                        }
                        else
                        {
                            Debug.WriteLine($"Login failed: {result.IsLockedOut}, {result.IsNotAllowed}, {result.RequiresTwoFactor}");
                        }
                    }
                    else
                    {
                        Debug.WriteLine("User not found");
                    }
                    
                    ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre");
                }
                
                return View(model);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Login error: {ex.Message}");
                ModelState.AddModelError("", $"Giriş hatası: {ex.Message}");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Yeni kullanıcıya "User" rolünü ekle
                    await _userManager.AddToRoleAsync(user, "User");
                    
                    // Kullanıcıyı otomatik olarak giriş yap
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    
                    // Başarılı kayıt mesajı
                    TempData["Success"] = "Hesabınız başarıyla oluşturuldu ve giriş yapıldı.";
                    
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            
            // Çerezleri temizle
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            
            // Oturumu temizle
            HttpContext.Session.Clear();
            
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
} 