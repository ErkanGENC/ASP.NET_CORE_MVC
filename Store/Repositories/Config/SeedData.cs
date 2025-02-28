using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public static class SeedData
    {
        public static void SeedDatabase(ModelBuilder builder)
        {
            // Kategoriler için seed data
            builder.Entity<Category>().HasData(
                new Category
                {
                    CategoryId = 1,
                    CategoryName = "Elektronik",
                    Description = "Elektronik ürünler"
                },
                new Category
                {
                    CategoryId = 2,
                    CategoryName = "Giyim",
                    Description = "Giyim ürünleri"
                }
            );

            // Roller için seed data
            builder.Entity<Role>().HasData(
                new Role
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Description = "Yönetici rolü"
                },
                new Role
                {
                    Id = "2",
                    Name = "User",
                    NormalizedName = "USER",
                    Description = "Kullanıcı rolü"
                }
            );

            // Admin kullanıcısı için seed data
            var hasher = new PasswordHasher<User>();
            builder.Entity<User>().HasData(
                new User
                {
                    Id = "1",
                    UserName = "admin@store.com",
                    NormalizedUserName = "ADMIN@STORE.COM",
                    Email = "admin@store.com",
                    NormalizedEmail = "ADMIN@STORE.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Admin123!"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    FirstName = "Admin",
                    LastName = "User"
                }
            );

            // Admin kullanıcısına Admin rolü atama
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "1",
                    UserId = "1"
                }
            );

            // Mevcut Category ve Product seed dataları
            // ... (mevcut seed verileriniz)
        }
    }
} 