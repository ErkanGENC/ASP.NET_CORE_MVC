using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApp.Models;
using StoreApp.Models.ViewModels;

namespace StoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private const string AdminEmail = "admin@store.com";

        public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            // Önce admin kullanıcısını kontrol et ve gerekirse yetkilerini geri yükle
            await CheckAndRestoreAdminRights();

            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    EmailConfirmed = user.EmailConfirmed,
                    Roles = roles.ToList(),
                    IsAdmin = user.Email == AdminEmail
                });
            }

            return View(userViewModels);
        }

        private async Task CheckAndRestoreAdminRights()
        {
            var adminUser = await _userManager.FindByEmailAsync(AdminEmail);
            if (adminUser != null)
            {
                var adminRoles = await _userManager.GetRolesAsync(adminUser);
                if (!adminRoles.Contains("Admin"))
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                    TempData["Success"] = "Admin kullanıcısının yetkileri otomatik olarak geri yüklendi.";
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> RestoreAdmin()
        {
            await CheckAndRestoreAdminRights();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            // Admin kullanıcısının düzenlenmesini engelle
            if (user.Email == AdminEmail)
            {
                TempData["Error"] = "Admin kullanıcısı düzenlenemez.";
                return RedirectToAction(nameof(Index));
            }

            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            var viewModel = new StoreApp.Models.ViewModels.UserEditViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                AllRoles = roles.Select(r => r.Name).ToList(),
                UserRoles = userRoles.ToList(),
                IsAdmin = user.Email == AdminEmail
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StoreApp.Models.ViewModels.UserEditViewModel model)
        {
            // Rolleri tekrar yükle
            var roles = await _roleManager.Roles.ToListAsync();
            model.AllRoles = roles.Select(r => r.Name).ToList();

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Form validasyonu başarısız. Lütfen tüm alanları kontrol edin.";
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            if (user.Email == AdminEmail)
            {
                TempData["Error"] = "Admin kullanıcısı düzenlenemez.";
                return RedirectToAction(nameof(Index));
            }

            bool isUpdated = false;

            // Şifre değiştirme işlemi (önce yap ki diğer değişiklikler etkilenmesin)
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (model.NewPassword != model.ConfirmNewPassword)
                {
                    ModelState.AddModelError("", "Şifreler eşleşmiyor.");
                    return View(model);
                }

                try
                {
                    // Önce eski şifreyi kaldır
                    var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                    if (!removePasswordResult.Succeeded)
                    {
                        foreach (var error in removePasswordResult.Errors)
                        {
                            ModelState.AddModelError("", "Eski şifre kaldırılırken hata: " + error.Description);
                        }
                        return View(model);
                    }

                    // Yeni şifreyi ekle
                    var addPasswordResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
                    if (!addPasswordResult.Succeeded)
                    {
                        foreach (var error in addPasswordResult.Errors)
                        {
                            ModelState.AddModelError("", "Yeni şifre eklenirken hata: " + error.Description);
                        }
                        return View(model);
                    }

                    isUpdated = true;
                    TempData["PasswordSuccess"] = "Şifre başarıyla güncellendi.";
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Şifre güncellenirken bir hata oluştu: " + ex.Message);
                    return View(model);
                }
            }

            // Temel bilgileri güncelle
            if (user.UserName != model.UserName || 
                user.Email != model.Email || 
                user.PhoneNumber != model.PhoneNumber || 
                user.EmailConfirmed != model.EmailConfirmed)
            {
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.EmailConfirmed = model.EmailConfirmed;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
                isUpdated = true;
            }

            // Rolleri güncelle
            try 
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                var selectedRoles = model.UserRoles ?? new List<string>();

                if (!currentRoles.OrderBy(r => r).SequenceEqual(selectedRoles.OrderBy(r => r)))
                {
                    if (currentRoles.Any())
                    {
                        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                        if (!removeResult.Succeeded)
                        {
                            ModelState.AddModelError("", "Roller kaldırılırken hata oluştu.");
                            return View(model);
                        }
                    }

                    if (selectedRoles.Any())
                    {
                        var addResult = await _userManager.AddToRolesAsync(user, selectedRoles);
                        if (!addResult.Succeeded)
                        {
                            ModelState.AddModelError("", "Roller eklenirken hata oluştu.");
                            return View(model);
                        }
                    }
                    isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Roller güncellenirken bir hata oluştu: " + ex.Message);
                return View(model);
            }

            if (isUpdated)
            {
                TempData["Success"] = "Kullanıcı bilgileri başarıyla güncellendi.";
            }
            else
            {
                TempData["Info"] = "Herhangi bir değişiklik yapılmadı.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            if (user.Email == AdminEmail)
            {
                TempData["Error"] = "Admin kullanıcısı silinemez.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Kullanıcı başarıyla silindi.";
            }
            else
            {
                TempData["Error"] = "Kullanıcı silinirken bir hata oluştu.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
} 
