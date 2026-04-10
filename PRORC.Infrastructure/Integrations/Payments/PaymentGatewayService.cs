using Microsoft.Extensions.Logging;
using PRORC.Domain.Interfaces.Integrations;

namespace PRORC.Infrastructure.Integrations.Payments
{
    public class PaymentGatewayService : IPaymentGateway
    {
        private readonly ILogger<PaymentGatewayService> _logger;

        public PaymentGatewayService(ILogger<PaymentGatewayService> logger)
        {
            _logger = logger;
        }

        // Intenta autorizar un pago
        // Si todo está bien, devuelve una referencia de pago simulada
        public async Task<string?> AuthorizePaymentAsync(decimal amount, string paymentMethod)
        {
            try
            {
                // Simula una pequeña espera como si fuera una API externa
                await Task.Delay(300);

                // Validaciones básicas
                if (amount <= 0)
                {
                    _logger.LogWarning("Payment authorization failed because amount is invalid.");
                    return null;
                }

                if (string.IsNullOrWhiteSpace(paymentMethod))
                {
                    _logger.LogWarning("Payment authorization failed because payment method is empty.");
                    return null;
                }

                // Si el método de pago tiene contenido y el monto es válido, autorizamos
                // Generamos una referencia única
                var paymentReference = $"PAY-{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

                _logger.LogInformation(
                    "Payment authorized successfully. Method: {PaymentMethod}, Amount: {Amount}, Reference: {Reference}.",
                    paymentMethod,
                    amount,
                    paymentReference);

                return paymentReference;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while authorizing payment.");
                throw;
            }
        }

        // Intenta reembolsar un pago usando la referencia
        public async Task<bool> RefundPaymentAsync(string paymentReference)
        {
            try
            {
                // Simula una pequeña espera como si fuera una API externa
                await Task.Delay(300);

                // Si la referencia no existe, no se puede reembolsar
                if (string.IsNullOrWhiteSpace(paymentReference))
                {
                    _logger.LogWarning("Refund failed because payment reference is empty.");
                    return false;
                }

                // Si hay referencia válida, devolvemos true
                _logger.LogInformation("Payment refunded successfully. Reference: {Reference}.", paymentReference);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while refunding payment with reference {Reference}.", paymentReference);
                throw;
            }
        }
    }
}