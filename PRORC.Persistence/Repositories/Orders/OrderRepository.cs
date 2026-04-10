using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRORC.Domain.Entities.Orders;
using PRORC.Domain.Enums;
using PRORC.Domain.Interfaces.Repositories;
using PRORC.Persistence.Base;
using PRORC.Persistence.Context;

namespace PRORC.Persistence.Repositories.Orders
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(PRORCContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory) { }


        // Obtiene todas las órdenes de un usuario
        public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId)
        {
            try
            {
                return await _context.Orders
                    .AsNoTracking()
                    .Where(o => o.UserId == userId)
                    .OrderByDescending(o => o.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders by UserId {UserId}.", userId);
                throw;
            }
        }

        // Obtiene la orden asociada a una reserva
        public async Task<Order?> GetByReservationIdAsync(int reservationId)
        {
            try
            {
                return await _context.Orders
                    .AsNoTracking()
                    .FirstOrDefaultAsync(o => o.ReservationId == reservationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order by ReservationId {ReservationId}.", reservationId);
                throw;
            }
        }

        // Obtiene órdenes por estado
        public async Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status)
        {
            try
            {
                return await _context.Orders
                    .AsNoTracking()
                    .Where(o => o.Status == status)
                    .OrderByDescending(o => o.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders by Status {Status}.", status);
                throw;
            }
        }

        // Obtiene los items de una orden
        public async Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(int orderId)
        {
            try
            {
                return await _context.OrderItems
                    .AsNoTracking()
                    .Where(oi => oi.OrderId == orderId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order items by OrderId {OrderId}.", orderId);
                throw;
            }
        }

        // Agrega un nuevo item a una orden
        public async Task<OrderItem> AddOrderItemAsync(OrderItem item)
        {
            try
            {
                await _context.OrderItems.AddAsync(item);
                await _context.SaveChangesAsync();

                _logger.LogInformation("OrderItem added successfully.");

                return item;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while adding OrderItem.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while adding OrderItem.");
                throw;
            }
        }
    }
}