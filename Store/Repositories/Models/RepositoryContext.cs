using Microsoft.EntityFrameworkCore;
using Entities.Models;

namespace Repositories.Models
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            base.OnModelCreating(modelBuilder);
        }
    }
} 