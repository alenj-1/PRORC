using PRORC.Application.DTOs.Payments;
using PRORC.Application.Interfaces;
using PRORC.Domain.Entities.Payments;
using PRORC.Domain.Enums;
using PRORC.Domain.Interfaces.Repositories;

namespace PRORC.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;

        public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
        }

        public async Task<PaymentDto> CreateAsync(PaymentRequest request)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order is null)
                throw new InvalidOperationException("The order does not exist.");

            if (await _paymentRepository.ExistsPaymentForOrderAsync(request.OrderId))
                throw new InvalidOperationException("A payment has already been made for this order.");

            var entity = new Payment
            {
                OrderId = request.OrderId,
                Amount = request.Amount,
                Status = PaymentStatusEnum.Pending,
                TransactionReference = string.Empty,
                PaymentDate = DateTime.UtcNow
            };

            await _paymentRepository.AddAsync(entity);
            return Map(entity);
        }

        public async Task<PaymentDto?> GetByOrderIdAsync(int orderId)
        {
            var payment = await _paymentRepository.GetPaymentByOrderIdAsync(orderId);
            return payment is null ? null : Map(payment);
        }

        public async Task AuthorizeAsync(int orderId, string transactionReference)
        {
            var payment = await _paymentRepository.GetPaymentByOrderIdAsync(orderId);
            if (payment is null)
                throw new InvalidOperationException("There is no payment for this order.");

            payment.Status = PaymentStatusEnum.Authorized;
            payment.TransactionReference = transactionReference;
            payment.PaymentDate = DateTime.UtcNow;

            await _paymentRepository.UpdateAsync(payment);
        }

        private static PaymentDto Map(Payment payment) => new()
        {
            Id = payment.Id,
            OrderId = payment.OrderId,
            Amount = payment.Amount,
            Status = payment.Status,
            TransactionReference = payment.TransactionReference,
            PaymentDate = payment.PaymentDate
        };
    }
}
