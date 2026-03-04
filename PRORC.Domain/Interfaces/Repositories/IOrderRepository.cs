using PRORC.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Domain.Interfaces.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order, int>
    {
    }
}
