using Entities.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repositories.Contracts;
using Repositories.Models;

namespace Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;
        private readonly Lazy<IProductRepository> _productRepository;

        public RepositoryManager(RepositoryContext context)
        {
            _context = context;
            _productRepository = new Lazy<IProductRepository>(() => new ProductRepository(context));
        }

        public IProductRepository Product => _productRepository.Value;

        public void Save() => _context.SaveChanges();

        public static void SeedDatabase(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<RepositoryContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
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
                    }
                );

                context.SaveChanges();
            }
        }
    }
} 