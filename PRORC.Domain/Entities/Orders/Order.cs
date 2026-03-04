using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Domain.Entities.Orders
{
    public class Order
    {
        public int Id { get; set; }

        public List<OrderItem> OrderItem { get; set; } = new();
    }
}
