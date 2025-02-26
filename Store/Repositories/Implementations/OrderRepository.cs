using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.Models;

namespace Repositories.Implementations
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(RepositoryContext context) : base(context)
        {
        }

        public IQueryable<Order> GetAllOrders(bool trackChanges) =>
            FindAll(trackChanges)
                .Include(o => o.Lines)
                .ThenInclude(l => l.Product);

        public Order GetOneOrder(int id, bool trackChanges) =>
            FindByCondition(o => o.OrderId.Equals(id), trackChanges)
                .Include(o => o.Lines)
                .ThenInclude(l => l.Product)
                .SingleOrDefault();

        public void CreateOrder(Order order) => Create(order);

        public void UpdateOrder(Order order) => Update(order);

        public void DeleteOrder(Order order) => Delete(order);

        // Async implementasyonlar
        public async Task<IEnumerable<Order>> GetAllOrdersAsync() =>
            await FindAll(false)
                .Include(o => o.Lines)
                .ThenInclude(l => l.Product)
                .ToListAsync();

        public async Task<Order> GetOrderByIdAsync(int id) =>
            await FindByCondition(o => o.OrderId.Equals(id), false)
                .Include(o => o.Lines)
                .ThenInclude(l => l.Product)
                .SingleOrDefaultAsync();

        public async Task CreateOrderAsync(Order order) =>
            await Task.Run(() => Create(order));

        public async Task UpdateOrderAsync(Order order) =>
            await Task.Run(() => Update(order));

        public async Task DeleteOrderAsync(int id)
        {
            var order = await GetOrderByIdAsync(id);
            if (order != null)
                await Task.Run(() => Delete(order));
        }
    }
} 