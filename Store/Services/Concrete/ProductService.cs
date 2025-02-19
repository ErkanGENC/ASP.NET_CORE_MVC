using Entities.Models;
using Repositories.Contracts;
using Services.Abstract;

namespace Services.Concrete
{
    public class ProductService : IProductService
    {
        private readonly IRepositoryManager _repositoryManager;

        public ProductService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _repositoryManager.Product.GetAllProductsAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _repositoryManager.Product.GetProductByIdAsync(id);
        }

        public async Task CreateProductAsync(Product product)
        {
            await _repositoryManager.Product.CreateProductAsync(product);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _repositoryManager.Product.UpdateProductAsync(product);
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            await _repositoryManager.Product.DeleteProductAsync(id);
            await _repositoryManager.SaveAsync();
        }
    }
} 