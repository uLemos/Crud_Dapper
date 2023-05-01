using Order.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Order.Domain.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task CreateAsync(OrderModel order);
        Task ClientItemAsync(OrderItemModel item);
        Task UpdateAsync(OrderModel order);
        Task UpdateItemAsync(OrderItemModel item);
        Task DeleteAsync(string orderId);
        Task DeleteItemAsync(string itemId);
        Task<OrderModel> GetByIdAsync(string orderId);
        Task<List<OrderModel>> ListByFilterAsync(string orderId = null, string name = null);
        Task<List<OrderItemModel>> ListItemOrderIdAsync(string orderId);
        Task<bool> ExistByIdAsync(string orderId);

    }
}
