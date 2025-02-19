using Services.Abstract;
using Repositories.Contracts;
using Entities.Models;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepositoryManager _repositoryManager;

        public CategoryService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _repositoryManager.Category.GetAllCategoriesAsync();
            return categories.Select(c => new CategoryDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                ProductCount = GetProductCountByCategoryAsync(c.CategoryId).Result
            });
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _repositoryManager.Category.GetCategoryByIdAsync(id);
            if (category == null)
                throw new Exception($"Category with id {id} not found");

            return new CategoryDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                ProductCount = await GetProductCountByCategoryAsync(id)
            };
        }

        public async Task CreateCategoryAsync(CategoryDto categoryDto)
        {
            var category = new Category
            {
                CategoryName = categoryDto.CategoryName
            };

            await _repositoryManager.Category.CreateCategoryAsync(category);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateCategoryAsync(CategoryDto categoryDto)
        {
            var category = await _repositoryManager.Category.GetCategoryByIdAsync(categoryDto.CategoryId);
            if (category == null)
                throw new Exception($"Category with id {categoryDto.CategoryId} not found");

            category.CategoryName = categoryDto.CategoryName;

            await _repositoryManager.Category.UpdateCategoryAsync(category);
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _repositoryManager.Category.DeleteCategoryAsync(id);
            await _repositoryManager.SaveAsync();
        }

        public async Task<int> GetProductCountByCategoryAsync(int categoryId)
        {
            var products = await _repositoryManager.Product.GetAllProductsAsync();
            return products.Count(p => p.CategoryId == categoryId);
        }
    }
} 