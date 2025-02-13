using Microsoft.EntityFrameworkCore;

namespace StoreApp.Models
{
    public class RepositoryContext: DbContext
    {
        public DbSet<Products>? Products { get; set; }
        public RepositoryContext(DbContextOptions<RepositoryContext> options)
        :base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Products>()
            .HasData(
                new Products{
                    Id = 1,
                    Name = "Computer",
                    Price = 1000
                },
                new Products{
                    Id = 2,
                    Name = "Laptop",
                    Price = 2000
                },
                new Products{
                    Id = 3,
                    Name = "Tablet",
                    Price = 3000,
                },  
                new Products{
                    Id = 4,
                    Name = "Smartphone",
                    Price = 4000
                }
            );
                
           
        }
    }
}
