using System;
using System.Data.Common;
using Repositories.Contracts;
using Services.Abstract;

namespace Services.Concrete
{
    public class ServiceManager : IServiceManager
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IOrderService _orderService;
        private readonly IRepositoryManager _repositoryManager;

        public ServiceManager(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
            _productService = new ProductService(repositoryManager);
            _categoryService = new CategoryService(repositoryManager);
            _orderService = new OrderService(repositoryManager);
        }

        public IProductService ProductService => _productService;
        public ICategoryService CategoryService => _categoryService;
        public IOrderService OrderService => _orderService;

        public DbConnection GetDbConnection()
        {
            return _repositoryManager.GetDbConnection();
        }
    }
} 