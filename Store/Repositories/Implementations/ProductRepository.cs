using Entities.Models;
using Repositories.Contracts;
using Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Implementations
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext context) : base(context)
        {
        }

        public IQueryable<Product> GetAllProducts(bool trackChanges) =>
            FindAll(trackChanges)
                .Include(p => p.Category);

        public Product? GetOneProduct(int id, bool trackChanges) =>
            FindByCondition(p => p.Id.Equals(id), trackChanges)
                .Include(p => p.Category)
                .SingleOrDefault();

        public void CreateProduct(Product product) => Create(product);

        public void UpdateProduct(Product product) => Update(product);

        public void DeleteProduct(Product product) => Delete(product);

        // Async implementasyonlar
        public async Task<IEnumerable<Product>> GetAllProductsAsync() =>
            await FindAll(false)
                .Include(p => p.Category)
                .ToListAsync();

        public async Task<Product?> GetProductByIdAsync(int id) =>
            await FindByCondition(p => p.Id.Equals(id), false)
                .Include(p => p.Category)
                .SingleOrDefaultAsync();

        public async Task CreateProductAsync(Product product) => 
            await Task.Run(() => Create(product));

        public async Task UpdateProductAsync(Product product) => 
            await Task.Run(() => Update(product));

        public async Task DeleteProductAsync(int id)
        {
            var product = await GetProductByIdAsync(id);
            if (product != null)
                await Task.Run(() => Delete(product));
        }
    }
} 