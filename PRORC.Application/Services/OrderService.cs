using PRORC.Application.DTOs.Orders;
using PRORC.Application.Interfaces;
using PRORC.Domain.Entities.Orders;
using PRORC.Domain.Enums;
using PRORC.Domain.Interfaces.Repositories;

namespace PRORC.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IPaymentRepository _paymentRepository;

        public OrderService(
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            IRestaurantRepository restaurantRepository,
            IPaymentRepository paymentRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _restaurantRepository = restaurantRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<OrderDto> CreateAsync(CreateOrderRequest request)
        {
            if (await _userRepository.GetByIdAsync(request.UserId) is null)
                throw new InvalidOperationException("The user does not exist.");

            if (await _restaurantRepository.GetByIdAsync(request.RestaurantId) is null)
                throw new InvalidOperationException("The restaurant does not exist.");

            if (request.Items is null || request.Items.Count == 0)
                throw new InvalidOperationException("The order needs to have at least, one item.");

            var total = request.Items.Sum(i => i.UnitPrice * i.Quantity);

            var entity = new Order
            {
                UserId = request.UserId,
                RestaurantId = request.RestaurantId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = total,
                Status = OrderStatusEnum.Pending,
                OrderItems = request.Items.Select(i => new OrderItem
                {
                    MenuItemId = i.MenuItemId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            await _orderRepository.AddAsync(entity);

            var saved = await _orderRepository.GetByIdWithItemsAsync(entity.Id) ?? entity;
            return Map(saved);
        }

        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdWithItemsAsync(id);
            return order is null ? null : Map(order);
        }

        public async Task<List<OrderDto>> GetByUserAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserAsync(userId);
            return orders.Select(Map).ToList();
        }

        public async Task<List<OrderDto>> GetByRestaurantAsync(int restaurantId)
        {
            var orders = await _orderRepository.GetOrdersByRestaurantAsync(restaurantId);
            return orders.Select(Map).ToList();
        }

        public async Task ConfirmAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order is null)
                throw new InvalidOperationException("The order does not exist.");

            var hasAuthorizedPayment = await _paymentRepository.HasAuthorizedPaymentAsync(orderId);
            if (!hasAuthorizedPayment)
                throw new InvalidOperationException("The order cannot be confirmed without an authorized payment.");

            order.Status = OrderStatusEnum.Confirmed;
            await _orderRepository.UpdateAsync(order);
        }

        private static OrderDto Map(Order order) => new()
        {
            Id = order.Id,
            UserId = order.UserId,
            RestaurantId = order.RestaurantId,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            OrderItems = order.OrderItems.Select(i => new OrderItemDto
            {
                Id = i.Id,
                MenuItemId = i.MenuItemId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };
    }
}
