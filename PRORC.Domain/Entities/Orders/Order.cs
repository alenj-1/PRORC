using PRORC.Domain.Enums;
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
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public OrderStatusEnum Status { get; set; } = OrderStatusEnum.Pending;

        public List<OrderItem> OrderItem { get; set; } = new();

        public void Confirm()
        {
            Status = OrderStatusEnum.Confirmed;
        }

        public void Complete()
        {
            Status = OrderStatusEnum.Completed;
        }

        public void Cancel()
        {
            Status = OrderStatusEnum.Cancelled;
        }
    }
}
