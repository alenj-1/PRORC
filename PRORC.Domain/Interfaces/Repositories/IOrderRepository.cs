using PRORC.Domain.Entities.Orders;
using PRORC.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Domain.Interfaces.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order, int>
    {
        Task<Order?> GetByIdWithItemsAsync(int id);
        Task<List<Order>> GetOrdersByUserAsync(int userId);
        Task<List<Order>> GetOrdersByRestaurantAsync(int restaurantId);
        Task<List<Order>> GetOrdersByStatusAsync(OrderStatusEnum status);
        Task<List<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<Order>> GetLatestOrdersByUserAsync(int userId, int count);
        Task<Order?> GetOrderWithItemsByUserAsync(int orderId, int userId);
        Task<bool> ExistsOrderForUserAsync(int orderId, int userId);
    }
}
