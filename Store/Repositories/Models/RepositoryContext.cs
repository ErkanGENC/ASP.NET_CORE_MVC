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
    }
} 