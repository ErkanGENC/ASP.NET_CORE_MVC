using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories;
using Repositories.Models;

namespace Repositories.Implementations
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        private readonly RepositoryContext _context;

        public ProductRepository(RepositoryContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Product> GetAllProducts(bool trackChanges) =>
            FindAll(trackChanges)
                .Include(p => p.Category);

        public Product? GetOneProduct(int id, bool trackChanges)
        {
            return FindByCondition(p => p.ProductId == id, trackChanges)
                .Include(p => p.Category)
                .SingleOrDefault();
        }

        public void CreateProduct(Product product) => Create(product);

        public void UpdateProduct(Product product) => Update(product);

        public void DeleteProduct(Product product) => Delete(product);

        // Async implementasyonlar
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await FindAll(false)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task CreateProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await GetProductByIdAsync(id);
            if (product is not null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
} 