using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Services.Abstract;

namespace StoreApp.Components
{
    public class ProductSummaryViewComponent : ViewComponent
    {
        private readonly IServiceManager _manager;

        public ProductSummaryViewComponent(IServiceManager manager)
        {
            _manager = manager;
        }

        public string Invoke()
        {
            return _manager.ProductService.GetAllProductsAsync().Result.Count().ToString();
        }
    }
} 