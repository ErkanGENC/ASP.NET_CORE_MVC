using Entities.Models;
using Repositories.Contracts;
using Repositories.Models;

namespace Repositories.Implementations
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext context) : base(context)
        {
        }

        public IQueryable<Product> GetAllProducts(bool trackChanges) =>
            FindAll(trackChanges);

        public Product? GetOneProduct(int id, bool trackChanges) =>
            FindByCondition(p => p.Id.Equals(id), trackChanges).SingleOrDefault();

        public void CreateProduct(Product product) => Create(product);

        public void UpdateProduct(Product product) => Update(product);

        public void DeleteProduct(Product product) => Delete(product);
    }
} 