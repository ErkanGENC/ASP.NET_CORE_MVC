using Entities.Models;
using Entities.Dtos;
using Repositories.Contracts;
using Services.Abstract;
using System.Linq;

namespace Services.Concrete
{
    public class ProductService : IProductService
    {
        private readonly IRepositoryManager _repositoryManager;

        public ProductService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _repositoryManager.Product.GetAllProductsAsync();
            return products.Select(p => new ProductDto
            {
                ProductId = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.CategoryName
            });
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _repositoryManager.Product.GetProductByIdAsync(id);
            if (product == null)
                throw new Exception($"Product with id {id} not found");

            return new ProductDto
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.CategoryName
            };
        }

        public async Task CreateProductAsync(ProductDtoForInsertion productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Description = productDto.Description,
                ImageUrl = productDto.ImageUrl ?? "/images/products/default.jpg",
                CategoryId = productDto.CategoryId
            };

            await _repositoryManager.Product.CreateProductAsync(product);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateProductAsync(ProductDtoForUpdate productDto)
        {
            var product = await _repositoryManager.Product.GetProductByIdAsync(productDto.ProductId);
            if (product == null)
                throw new Exception($"Product with id {productDto.ProductId} not found");

            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Description = productDto.Description;
            product.ImageUrl = productDto.ImageUrl;
            product.CategoryId = productDto.CategoryId;

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