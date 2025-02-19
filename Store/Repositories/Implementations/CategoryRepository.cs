using Entities.Models;
using Repositories.Contracts;
using Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Implementations
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(RepositoryContext context) : base(context)
        {
        }

        public IQueryable<Category> GetAllCategories(bool trackChanges) =>
            FindAll(trackChanges);

        public Category GetOneCategoryById(int id, bool trackChanges) =>
            FindByCondition(c => c.CategoryId.Equals(id), trackChanges).SingleOrDefault();

        public void CreateCategory(Category category) => Create(category);

        public void UpdateCategory(Category category) => Update(category);

        public void DeleteCategory(Category category) => Delete(category);

        // Async metodlar
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync() =>
            await FindAll(false).ToListAsync();

        public async Task<Category> GetCategoryByIdAsync(int id) =>
            await FindByCondition(c => c.CategoryId.Equals(id), false).SingleOrDefaultAsync();

        public async Task CreateCategoryAsync(Category category) =>
            await Task.Run(() => Create(category));

        public async Task UpdateCategoryAsync(Category category) =>
            await Task.Run(() => Update(category));

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await GetCategoryByIdAsync(id);
            if (category != null)
                await Task.Run(() => Delete(category));
        }
    }
} 