using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using StoreApp.Models.ViewModels;
using System.Diagnostics;
using StoreApp.Models;

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
                if (ModelState.IsValid)
                {
                    Debug.WriteLine($"Login attempt for: {model.Email}");
                    
                    // Önce e-posta ile kullanıcıyı bulmayı dene
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    
                    // E-posta ile bulunamadıysa, kullanıcı adı ile dene
                    if (user == null)
                    {
                        user = await _userManager.FindByNameAsync(model.Email);
                    }

                    if (user != null)
                    {
                        Debug.WriteLine($"User found: {user.UserName}, Email: {user.Email}");
                        
                        await _signInManager.SignOutAsync();
                        
                        var result = await _signInManager.PasswordSignInAsync(
                            userName: user.UserName,
                            password: model.Password, 
                            isPersistent: model.RememberMe, 
                            lockoutOnFailure: true);
                            
                        Debug.WriteLine($"Login result: {result.Succeeded}");
                            
                        if (result.Succeeded)
                        {
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
                            if (result.IsLockedOut)
                            {
                                ModelState.AddModelError("", "Hesabınız kilitlendi. Lütfen daha sonra tekrar deneyin.");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Geçersiz kullanıcı adı/e-posta veya şifre");
                            }
                        }
                    }
                    else
                    {
                        Debug.WriteLine("User not found");
                        ModelState.AddModelError("", "Geçersiz kullanıcı adı/e-posta veya şifre");
                    }
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
        public async Task<IActionResult> Register([FromForm] RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
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