using PRORC.Domain.Interfaces.Integrations;
using PRORC.Domain.Interfaces.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Infrastructure.Integrations.Payments
{
    public class PaymentGatewayService : IPaymentGateway
    {
        private readonly IAuditLogger _auditLogger;

        public PaymentGatewayService(IAuditLogger auditLogger)
        {
            _auditLogger = auditLogger;
        }

        public async Task<string> AuthorizedAsync(decimal amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException("The Payment amount must be greater than zero.");

            var transactionReference = $"TXN-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N}".ToUpper();

            await _auditLogger.LogAsync("PaymentAuthorized", $"Payment authorized for amount {amount:F2}, Transaction: {transactionReference}");

            return transactionReference;
        }
    }
}
