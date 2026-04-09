using Microsoft.Extensions.Logging;
using PRORC.Application.DTOs.Orders;
using PRORC.Application.Interfaces;
using PRORC.Domain.Entities.Orders;
using PRORC.Domain.Enums;
using PRORC.Domain.Interfaces.Logging;
using PRORC.Domain.Interfaces.Repositories;

namespace PRORC.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAuditLogger _auditLogger;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository orderRepository,
            IReservationRepository reservationRepository,
            IMenuRepository menuRepository,
            IPaymentRepository paymentRepository,
            IAuditLogger auditLogger,
            ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _reservationRepository = reservationRepository;
            _menuRepository = menuRepository;
            _paymentRepository = paymentRepository;
            _auditLogger = auditLogger;
            _logger = logger;
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderRequest request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request));

                if (request.Items == null || request.Items.Count == 0)
                    throw new InvalidOperationException("The order must contain at least one item.");

                var reservation = await _reservationRepository.GetByIdAsync(request.ReservationId)
                    ?? throw new KeyNotFoundException("Reservation not found.");

                if (reservation.UserId != request.UserId)
                    throw new InvalidOperationException("The order does not belong to that user.");

                if (reservation.Status == ReservationStatus.Cancelled)
                    throw new InvalidOperationException("You cannot create an order for a cancelled reservation.");

                var existingOrder = await _orderRepository.GetByReservationIdAsync(request.ReservationId);
                if (existingOrder != null)
                    throw new InvalidOperationException("That reservation already has an order.");

                var activeMenu = await _menuRepository.GetActiveByRestaurantIdAsync(reservation.RestaurantId)
                    ?? throw new InvalidOperationException("The restaurant does not have an active menu.");

                var order = Order.Create(request.UserId, request.ReservationId);
                var createdOrder = await _orderRepository.AddAsync(order);

                var createdItems = new List<OrderItem>();

                foreach (var requestItem in request.Items)
                {
                    var menuItem = await _menuRepository.GetMenuItemByIdAsync(requestItem.MenuItemId)
                        ?? throw new KeyNotFoundException($"Menu item {requestItem.MenuItemId} was not found.");

                    if (!menuItem.IsAvailable)
                        throw new InvalidOperationException($"The item {menuItem.Name} is not available.");

                    if (menuItem.MenuId != activeMenu.Id)
                        throw new InvalidOperationException($"The item {menuItem.Name} does not belong to the active menu.");

                    if (requestItem.Quantity <= 0)
                        throw new InvalidOperationException("The quantity must be greater than zero.");

                    var orderItem = OrderItem.Create(
                        createdOrder.Id,
                        menuItem.Id,
                        menuItem.Name,
                        requestItem.Quantity,
                        menuItem.Price);

                    var savedItem = await _orderRepository.AddOrderItemAsync(orderItem);
                    createdItems.Add(savedItem);
                }

                createdOrder.CalculateTotal(createdItems);
                await _orderRepository.UpdateAsync(createdOrder);

                _logger.LogInformation("Order {OrderId} created successfully.", createdOrder.Id);

                await TryWriteAuditAsync(
                    createdOrder.UserId,
                    "CreateOrder",
                    "Order",
                    createdOrder.Id,
                    $"Order created with total amount {createdOrder.TotalAmount}.");

                return await BuildOrderDtoAsync(createdOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order for reservation {ReservationId}.", request?.ReservationId);
                throw;
            }
        }

        public async Task<OrderDto?> GetByIdAsync(int orderId)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId);
                return order == null ? null : await BuildOrderDtoAsync(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order {OrderId}.", orderId);
                throw;
            }
        }

        public async Task<OrderDto?> GetByReservationIdAsync(int reservationId)
        {
            try
            {
                var order = await _orderRepository.GetByReservationIdAsync(reservationId);
                return order == null ? null : await BuildOrderDtoAsync(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order by reservation {ReservationId}.", reservationId);
                throw;
            }
        }

        public async Task<IEnumerable<OrderDto>> GetByUserIdAsync(int userId)
        {
            try
            {
                var orders = await _orderRepository.GetByUserIdAsync(userId);
                var result = new List<OrderDto>();

                foreach (var order in orders)
                {
                    result.Add(await BuildOrderDtoAsync(order));
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders for user {UserId}.", userId);
                throw;
            }
        }

        public async Task ConfirmOrderAsync(int orderId)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId)
                    ?? throw new KeyNotFoundException("Order not found.");

                var hasAuthorizedPayment = await _paymentRepository.HasAuthorizedPaymentAsync(orderId);

                order.Confirm(hasAuthorizedPayment);

                await _orderRepository.UpdateAsync(order);

                await TryWriteAuditAsync(order.UserId, "ConfirmOrder", "Order", order.Id, "Order confirmed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming order {OrderId}.", orderId);
                throw;
            }
        }

        public async Task CompleteOrderAsync(int orderId)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId)
                    ?? throw new KeyNotFoundException("Order not found.");

                order.Complete();

                await _orderRepository.UpdateAsync(order);

                await TryWriteAuditAsync(order.UserId, "CompleteOrder", "Order", order.Id, "Order completed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing order {OrderId}.", orderId);
                throw;
            }
        }

        public async Task CancelOrderAsync(int orderId)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId)
                    ?? throw new KeyNotFoundException("Order not found.");

                order.Cancel();

                await _orderRepository.UpdateAsync(order);

                await TryWriteAuditAsync(order.UserId, "CancelOrder", "Order", order.Id, "Order cancelled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling order {OrderId}.", orderId);
                throw;
            }
        }

        private async Task<OrderDto> BuildOrderDtoAsync(Order order)
        {
            var items = await _orderRepository.GetItemsByOrderIdAsync(order.Id);

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                ReservationId = order.ReservationId,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt,
                OrderItems = items.Select(i => new OrderItemDto
                {
                    MenuItemId = i.MenuItemId,
                    ItemName = i.ItemName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Subtotal = i.Subtotal
                }).ToList()
            };
        }

        private async Task TryWriteAuditAsync(int? userId, string action, string entityName, int entityId, string details)
        {
            try
            {
                await _auditLogger.LogAsync(userId, action, entityName, entityId, details);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Audit logging failed in OrderService.");
            }
        }
    }
}