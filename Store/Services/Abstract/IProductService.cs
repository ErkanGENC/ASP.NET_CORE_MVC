using Entities.Models;
using Entities.Dtos;

namespace Services.Abstract
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
        Task CreateProductAsync(ProductDtoForInsertion productDto);
        Task UpdateProductAsync(ProductDtoForUpdate productDto);
        Task DeleteProductAsync(int id);
    }
} 