using Repositories.Contracts;
using System.Data.Common;

namespace Repositories.Contracts
{
    public interface IRepositoryManager
    {
        IProductRepository Product { get; }
        ICategoryRepository Category { get; }
        IOrderRepository Order { get; }
        void Save();
        Task SaveAsync();
        DbConnection GetDbConnection();
    }
} 