using Services.Abstract;
using Repositories.Contracts;
using Entities.Models;

namespace Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepositoryManager _repositoryManager;

        public CategoryService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _repositoryManager.Category.GetAllCategoriesAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _repositoryManager.Category.GetCategoryByIdAsync(id);
        }

        public async Task CreateCategoryAsync(Category category)
        {
            await _repositoryManager.Category.CreateCategoryAsync(category);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            await _repositoryManager.Category.UpdateCategoryAsync(category);
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _repositoryManager.Category.DeleteCategoryAsync(id);
            await _repositoryManager.SaveAsync();
        }
    }
} 