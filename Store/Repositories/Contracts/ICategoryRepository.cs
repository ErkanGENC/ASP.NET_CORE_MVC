using Entities.Models;

namespace Repositories.Contracts
{
    public interface ICategoryRepository
    {
        IQueryable<Category> GetAllCategories(bool trackChanges);
        Category GetOneCategoryById(int id, bool trackChanges);
        void CreateCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
        
        // Async metodlar
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
    }
} 