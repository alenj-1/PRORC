using PRORC.Domain.Entities.Orders;
using PRORC.Domain.Enums;

namespace PRORC.Domain.Interfaces.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<IEnumerable<Order>> GetByUserIdAsync(int userId);
        Task<Order?> GetByReservationIdAsync(int reservationId);
        Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status);
        Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(int orderId);
    }
}