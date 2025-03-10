using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace Repositories.Models
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product - Category ilişkisi
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // Product varsayılan değerleri
            modelBuilder.Entity<Product>()
                .Property(p => p.ImageUrl)
                .HasDefaultValue("/images/products/default.jpg");

            modelBuilder.Entity<Product>()
                .Property(p => p.Description)
                .HasDefaultValue(string.Empty);

            // Category validasyonları
            modelBuilder.Entity<Category>()
                .Property(c => c.CategoryName)
                .IsRequired()
                .HasMaxLength(100);

            // Order - OrderLine ilişkisi
            modelBuilder.Entity<OrderLine>()
                .HasOne(ol => ol.Order)
                .WithMany(o => o.Lines)
                .HasForeignKey(ol => ol.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // OrderLine - Product ilişkisi
            modelBuilder.Entity<OrderLine>()
                .HasOne(ol => ol.Product)
                .WithMany()
                .HasForeignKey(ol => ol.ProductId)
                .OnDelete(DeleteBehavior.SetNull);

            // Identity ilişkili tablolar için anahtar tanımlamaları
            modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserRole<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserToken<string>>().HasNoKey();

            // Seed verilerini ekle
            SeedData.SeedDatabase(modelBuilder);
        }
    }
} 