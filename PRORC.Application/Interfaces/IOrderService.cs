using PRORC.Application.DTOs.Orders;

namespace PRORC.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(CreateOrderRequest request);
        Task<OrderDto?> GetByIdAsync(int orderId);
        Task<OrderDto?> GetByReservationIdAsync(int reservationId);
        Task<IEnumerable<OrderDto>> GetByUserIdAsync(int userId);
        Task ConfirmOrderAsync(int orderId);
        Task CompleteOrderAsync(int orderId);
        Task CancelOrderAsync(int orderId);
    }
}