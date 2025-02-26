using Services.Abstract;
using System.Data.Common;

namespace Services.Abstract
{
    public interface IServiceManager
    {
        IProductService ProductService { get; }
        ICategoryService CategoryService { get; }
        IOrderService OrderService { get; }
        DbConnection GetDbConnection();
    }
} 