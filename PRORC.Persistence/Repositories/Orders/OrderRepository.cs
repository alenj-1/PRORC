using Microsoft.EntityFrameworkCore;
using PRORC.Domain.Entities.Orders;
using PRORC.Domain.Enums;
using PRORC.Domain.Interfaces.Repositories;
using PRORC.Persistence.Base;
using PRORC.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Persistence.Repositories.Orders
{
    public class OrderRepository : BaseRepository<Order, int>, IOrderRepository
    {
        public OrderRepository(PRORCContext context) : base(context) 
        {
        }

        public async Task<Order?> GetByIdWithItemsAsync(int id)
        {
            return await _dbSet
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Order>> GetOrdersByUserAsync(int userId)
        {
            return await _dbSet
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByRestaurantAsync(int restaurantId)
        {
            return await _dbSet
                .Where(o => o.RestaurantId == restaurantId)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByStatusAsync(OrderStatusEnum status)
        {
            return await _dbSet
                .Where(o => o.Status == status)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<List<Order>> GetLatestOrdersByUserAsync(int userId, int count)
        {
            return await _dbSet
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderWithItemsByUserAsync(int orderId, int userId)
        {
            return await _dbSet
                .Where(o => o.Id == orderId && o.UserId == userId)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsOrderForUserAsync(int orderId, int userId)
        {
            return await _dbSet
                .AnyAsync(o => o.Id == orderId && o.UserId == userId);
        }
    }
}
