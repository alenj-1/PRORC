using PRORC.Domain.Enums;

namespace PRORC.Domain.Entities.Orders
{
    public class Order
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public int ReservationId { get; private set; }
        public decimal TotalAmount { get; private set; }
        public OrderStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }


        private Order() { }

        public static Order Create(int userId, int reservationId)
        {
            // Validaciones de negocio
            if (userId <= 0)
                throw new ArgumentException("The order must have a valid customer.");

            if (reservationId <= 0)
                throw new ArgumentException("The order must be linked to a valid reservation.");

            return new Order
            {
                 UserId = userId,
                 ReservationId = reservationId,
                 TotalAmount = 0,
                 Status = OrderStatus.Pending,
                 CreatedAt = DateTime.UtcNow
            };
        }


        public void CalculateTotal(List<OrderItem> items)
        {
            if (items == null || items.Count == 0)
                throw new ArgumentException("The order must contain at least one item.");

            TotalAmount = items.Sum(i => i.Subtotal);

            if (TotalAmount <= 0)
                throw new InvalidOperationException("The total amount must be greater than zero.");
        }


        public void Confirm(bool paymentAuthorized)
        {
            if (Status != OrderStatus.Pending)
                throw new InvalidOperationException("Only orders with Pending status can be confirmed");

            if (TotalAmount <= 0)
                throw new InvalidOperationException("An order cannot be confirmed without a calculated total");

            if (!paymentAuthorized)
                throw new InvalidOperationException("An order cannot be confirmed without an authorized payment.");

            Status = OrderStatus.Confirmed;
        }


        public void Complete()
        {
            if (Status != OrderStatus.Confirmed)
                throw new InvalidOperationException("Only confirmed orders can be fulfilled.");

            Status = OrderStatus.Completed;
        }


        public void Cancel()
        {
            if (Status == OrderStatus.Completed)
                throw new InvalidOperationException("You cannot cancel a completed order.");

            if (Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("The order has already been canceled.");

            Status = OrderStatus.Cancelled;
        }
    }
}