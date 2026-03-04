using Microsoft.EntityFrameworkCore;
using PRORC.Domain.Entities.Orders;
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
        public OrderRepository(PRORCContext context) : base(context) { }

        public Task<Order?> GetByIdWithItemsAsync(int id)
        {
            return _dbSet
                .Include(o => o.OrderItem)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}
