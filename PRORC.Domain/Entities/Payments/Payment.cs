using PRORC.Domain.Enums;

namespace PRORC.Domain.Entities.Payments
{
    public class Payment
    {
        public int Id { get; private set; }
        public int OrderId { get; private set; }
        public decimal Amount { get; private set; }
        public PaymentStatus Status { get; private set; }
        public string PaymentMethod { get; private set; } = string.Empty;
        public DateTime PaymentDate { get; private set; }
        public string? PaymentReference { get; private set; }


        private Payment() { }

        public static Payment Create(int orderId, decimal amount, string paymentMethod)
        {
            // Validaciones de negocio
            if (orderId <= 0)
                throw new ArgumentException("The payment must be associated with a valid order.");
            
            if (amount <= 0)
                throw new ArgumentException("The payment amount must be greater than 0.");

            if (string.IsNullOrWhiteSpace(paymentMethod))
                throw new ArgumentException("The payment method cannot be empty.");

            return new Payment
            {
                OrderId = orderId,
                Amount = amount,
                PaymentMethod = paymentMethod,
                Status = PaymentStatus.Pending,
                PaymentDate = DateTime.UtcNow,
                PaymentReference = null
            };
        }

        public void Authorize(string paymentReference)
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Only payments with Pending status can be authorized.");

            if (string.IsNullOrWhiteSpace(paymentReference))
                throw new ArgumentException("The payment reference cannot be empty.");

            Status = PaymentStatus.Authorized;
            PaymentReference = paymentReference;
            PaymentDate = DateTime.UtcNow;
        }

        public void Reject()
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Only payments with Pending status can be rejected.");

            Status = PaymentStatus.Rejected;
            PaymentDate = DateTime.UtcNow;
        }

        public void Refund()
        {
            // Solo se puede reembolsar un pago ya autorizado
            if (Status != PaymentStatus.Authorized)
                throw new InvalidOperationException("Only authorized payments can be refunded.");

            Status = PaymentStatus.Refunded;
            PaymentDate = DateTime.UtcNow;
        }
    }
}