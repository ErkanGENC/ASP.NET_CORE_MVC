using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace Repositories.Models
{
    public class RepositoryContext : IdentityDbContext<IdentityUser>
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

            // Identity tabloları için tablo adları ve şema ayarları
            modelBuilder.Entity<IdentityUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

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
                
            // Order sütunları için nullable olmayan özellikler belirle
            modelBuilder.Entity<Order>()
                .Property(o => o.Name)
                .IsRequired();
                
            modelBuilder.Entity<Order>()
                .Property(o => o.LastName)
                .IsRequired();
                
            modelBuilder.Entity<Order>()
                .Property(o => o.Address1)
                .IsRequired();
                
            modelBuilder.Entity<Order>()
                .Property(o => o.City)
                .IsRequired();
                
            modelBuilder.Entity<Order>()
                .Property(o => o.District)
                .IsRequired();
                
            modelBuilder.Entity<Order>()
                .Property(o => o.PostalCode)
                .IsRequired();
                
            modelBuilder.Entity<Order>()
                .Property(o => o.Phone)
                .IsRequired();
                
            modelBuilder.Entity<Order>()
                .Property(o => o.Email)
                .IsRequired();
        }
    }
} 