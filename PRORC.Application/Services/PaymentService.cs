using Microsoft.Extensions.Logging;
using PRORC.Application.DTOs.Payments;
using PRORC.Application.Interfaces;
using PRORC.Domain.Entities.Payments;
using PRORC.Domain.Interfaces.Integrations;
using PRORC.Domain.Interfaces.Logging;
using PRORC.Domain.Interfaces.Repositories;

namespace PRORC.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentGateway _paymentGateway;
        private readonly IAuditLogger _auditLogger;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IOrderRepository orderRepository,
            IPaymentGateway paymentGateway,
            IAuditLogger auditLogger,
            ILogger<PaymentService> logger)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _paymentGateway = paymentGateway;
            _auditLogger = auditLogger;
            _logger = logger;
        }

        public async Task<PaymentDto> CreatePaymentAsync(PaymentRequest request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request));

                var order = await _orderRepository.GetByIdAsync(request.OrderId)
                    ?? throw new KeyNotFoundException("Order not found.");

                if (order.TotalAmount <= 0)
                    throw new InvalidOperationException("The order total amount must be calculated before creating a payment.");

                var existingPayment = await _paymentRepository.GetByOrderIdAsync(request.OrderId);
                if (existingPayment != null)
                    throw new InvalidOperationException("That order already has a payment.");

                var payment = Payment.Create(order.Id, order.TotalAmount, request.PaymentMethod);

                var createdPayment = await _paymentRepository.AddAsync(payment);

                _logger.LogInformation("Payment {PaymentId} created successfully.", createdPayment.Id);

                await TryWriteAuditAsync(order.UserId, "CreatePayment", "Payment", createdPayment.Id, "Payment created.");

                return MapPayment(createdPayment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment for order {OrderId}.", request?.OrderId);
                throw;
            }
        }

        public async Task<PaymentDto?> GetByOrderIdAsync(int orderId)
        {
            try
            {
                var payment = await _paymentRepository.GetByOrderIdAsync(orderId);
                return payment == null ? null : MapPayment(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payment for order {OrderId}.", orderId);
                throw;
            }
        }

        public async Task<PaymentDto> AuthorizePaymentAsync(int paymentId)
        {
            try
            {
                var payment = await _paymentRepository.GetByIdAsync(paymentId)
                    ?? throw new KeyNotFoundException("Payment not found.");

                try
                {
                    // Intentamos autorizar el pago con el proveedor externo.
                    var reference = await _paymentGateway.AuthorizePaymentAsync(payment.Amount, payment.PaymentMethod);

                    if (string.IsNullOrWhiteSpace(reference))
                    {
                        payment.Reject();
                    }
                    else
                    {
                        payment.Authorize(reference);
                    }
                }
                catch (Exception ex)
                {
                    // Si falla la pasarela, rechazamos el pago localmente y lo registramos.
                    _logger.LogWarning(ex, "External payment gateway failed for payment {PaymentId}.", paymentId);
                    payment.Reject();
                }

                await _paymentRepository.UpdateAsync(payment);

                await TryWriteAuditAsync(
                    null,
                    "AuthorizePayment",
                    "Payment",
                    payment.Id,
                    $"Payment status changed to {payment.Status}.");

                return MapPayment(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error authorizing payment {PaymentId}.", paymentId);
                throw;
            }
        }

        public async Task RejectPaymentAsync(int paymentId)
        {
            try
            {
                var payment = await _paymentRepository.GetByIdAsync(paymentId)
                    ?? throw new KeyNotFoundException("Payment not found.");

                payment.Reject();

                await _paymentRepository.UpdateAsync(payment);

                await TryWriteAuditAsync(null, "RejectPayment", "Payment", payment.Id, "Payment rejected manually.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting payment {PaymentId}.", paymentId);
                throw;
            }
        }

        public async Task RefundPaymentAsync(int paymentId)
        {
            try
            {
                var payment = await _paymentRepository.GetByIdAsync(paymentId)
                    ?? throw new KeyNotFoundException("Payment not found.");

                if (string.IsNullOrWhiteSpace(payment.PaymentReference))
                    throw new InvalidOperationException("The payment does not have a valid reference for refund.");

                var refunded = await _paymentGateway.RefundPaymentAsync(payment.PaymentReference);

                if (!refunded)
                    throw new InvalidOperationException("The payment gateway could not refund the payment.");

                payment.Refund();

                await _paymentRepository.UpdateAsync(payment);

                await TryWriteAuditAsync(null, "RefundPayment", "Payment", payment.Id, "Payment refunded.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refunding payment {PaymentId}.", paymentId);
                throw;
            }
        }

        private PaymentDto MapPayment(Payment payment)
        {
            return new PaymentDto
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                Status = payment.Status.ToString(),
                PaymentMethod = payment.PaymentMethod,
                PaymentDate = payment.PaymentDate,
                PaymentReference = payment.PaymentReference
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
                _logger.LogWarning(ex, "Audit logging failed in PaymentService.");
            }
        }
    }
}