using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace StoreApp.Models
{
    public static class IdentitySeed
    {
        public static async Task SeedRolesAsync(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            Debug.WriteLine("Rol oluşturma başladı...");

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var result = await roleManager.CreateAsync(new IdentityRole("Admin"));
                Debug.WriteLine($"Admin rolü oluşturuldu: {result.Succeeded}");
            }
            
            if (!await roleManager.RoleExistsAsync("User"))
            {
                var result = await roleManager.CreateAsync(new IdentityRole("User"));
                Debug.WriteLine($"User rolü oluşturuldu: {result.Succeeded}");
            }
        }

        public static async Task SeedAdminAsync(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string adminEmail = "admin@store.com";
            Debug.WriteLine($"Admin kullanıcısı oluşturma başladı: {adminEmail}");

            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                var adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                Debug.WriteLine($"Admin kullanıcısı oluşturuldu: {result.Succeeded}");

                if (result.Succeeded)
                {
                    var roleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
                    Debug.WriteLine($"Admin rolü atandı: {roleResult.Succeeded}");

                    if (!roleResult.Succeeded)
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            Debug.WriteLine($"Rol atama hatası: {error.Description}");
                        }
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Debug.WriteLine($"Kullanıcı oluşturma hatası: {error.Description}");
                    }
                }
            }
            else
            {
                Debug.WriteLine("Admin kullanıcısı zaten mevcut");
                if (!await userManager.IsInRoleAsync(admin, "Admin"))
                {
                    var roleResult = await userManager.AddToRoleAsync(admin, "Admin");
                    Debug.WriteLine($"Mevcut kullanıcıya Admin rolü atandı: {roleResult.Succeeded}");
                }
            }
        }
    }
} 