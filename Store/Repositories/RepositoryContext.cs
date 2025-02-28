using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class RepositoryContext : IdentityDbContext<User, Role, string>
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mevcut konfigürasyonlar
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            // Seed data için örnek admin kullanıcı ve rol
            SeedData.SeedDatabase(modelBuilder);
        }
    }
} 