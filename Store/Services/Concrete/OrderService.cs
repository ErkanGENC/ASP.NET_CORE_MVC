using Entities.Models;
using Repositories.Contracts;
using Services.Abstract;

namespace Services.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly IRepositoryManager _repositoryManager;

        public OrderService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _repositoryManager.Order.GetAllOrdersAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var order = await _repositoryManager.Order.GetOrderByIdAsync(id);
            if (order == null)
                throw new Exception($"Order with id {id} not found");

            return order;
        }

        public async Task CreateOrderAsync(Order order)
        {
            await _repositoryManager.Order.CreateOrderAsync(order);
            await _repositoryManager.SaveAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            await _repositoryManager.Order.UpdateOrderAsync(order);
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            await _repositoryManager.Order.DeleteOrderAsync(id);
            await _repositoryManager.SaveAsync();
        }
    }
} 