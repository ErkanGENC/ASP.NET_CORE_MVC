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

        public RepositoryManager(RepositoryContext context)
        {
            _context = context;
            _productRepository = new ProductRepository(context);
            _categoryRepository = new CategoryRepository(context);
        }

        public IProductRepository Product => _productRepository;
        public ICategoryRepository Category => _categoryRepository;

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
                    }
                );

                context.SaveChanges();
            }

            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product
                    {
                        Name = "Bilgisayar",
                        Price = 15000,
                        Description = "Gaming Laptop",
                        ImageUrl = "/images/products/laptop.jpg"
                    },
                    new Product
                    {
                        Name = "Telefon",
                        Price = 7000,
                        Description = "Akıllı Telefon",
                        ImageUrl = "/images/products/phone.jpg"
                    },
                    new Product
                    {
                        Name = "Tablet",
                        Price = 5000,
                        Description = "10 inç Tablet",
                        ImageUrl = "/images/products/tablet.jpg"
                    },
                    new Product
                    {
                        Name = "Lazer",
                        Price = 300,
                        Description = "Lazer",
                        ImageUrl = "/images/products/default.jpg"
                    }
                );

                context.SaveChanges();
            }
        }
    }
} 