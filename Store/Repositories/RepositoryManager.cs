using Entities.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repositories.Contracts;
using Repositories.Implementations;
using Repositories.Models;
using System.Data.Common;

namespace Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IOrderRepository _orderRepository;

        public RepositoryManager(RepositoryContext context)
        {
            _context = context;
            _productRepository = new ProductRepository(context);
            _categoryRepository = new CategoryRepository(context);
            _orderRepository = new OrderRepository(context);
        }

        public IProductRepository Product => _productRepository;
        public ICategoryRepository Category => _categoryRepository;
        public IOrderRepository Order => _orderRepository;

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public DbConnection GetDbConnection()
        {
            return _context.Database.GetDbConnection();
        }

        public static void SeedDatabase(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<RepositoryContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category
                    {
                        CategoryName = "Elektronik"
                    },
                    new Category
                    {
                        CategoryName = "Kitaplar"
                    },
                    new Category
                    {
                        CategoryName = "Giyim"
                    },
                    new Category
                    {
                        CategoryName = "Spor"
                    }
                );

                context.SaveChanges();
            }

            if (!context.Products.Any())
            {
                var elektronik = context.Categories.First(c => c.CategoryName == "Elektronik");
                var kitaplar = context.Categories.First(c => c.CategoryName == "Kitaplar");

                context.Products.AddRange(
                    new Product
                    {
                        Name = "Gaming Laptop",
                        Price = 15000,
                        Description = "Yüksek performanslı oyun bilgisayarı",
                        ImageUrl = "/images/products/laptop.jpg",
                        CategoryId = elektronik.CategoryId
                    },
                    new Product
                    {
                        Name = "Akıllı Telefon",
                        Price = 7000,
                        Description = "Son model akıllı telefon",
                        ImageUrl = "/images/products/phone.jpg",
                        CategoryId = elektronik.CategoryId
                    },
                    new Product
                    {
                        Name = "Tablet",
                        Price = 5000,
                        Description = "10 inç ekranlı tablet",
                        ImageUrl = "/images/products/tablet.jpg",
                        CategoryId = elektronik.CategoryId
                    },
                    new Product
                    {
                        Name = "C# Programlama",
                        Price = 150,
                        Description = "C# programlama dili kitabı",
                        ImageUrl = "/images/products/book.jpg",
                        CategoryId = kitaplar.CategoryId
                    },
                    new Product
                    {
                        Name = "ASP.NET Core MVC",
                        Price = 180,
                        Description = "Web programlama kitabı",
                        ImageUrl = "/images/products/book.jpg",
                        CategoryId = kitaplar.CategoryId
                    }
                );

                context.SaveChanges();
            }
        }
    }
} 