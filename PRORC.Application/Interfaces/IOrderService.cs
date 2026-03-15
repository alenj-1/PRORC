using PRORC.Application.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateAsync(CreateOrderRequest request);
        Task<OrderDto?> GetByIdAsync(int id);
        Task<List<OrderDto>> GetByUserAsync(int userId);
        Task<List<OrderDto>> GetByRestaurantAsync(int restaurantId);
        Task ConfirmAsync(int orderId);
    }
}
