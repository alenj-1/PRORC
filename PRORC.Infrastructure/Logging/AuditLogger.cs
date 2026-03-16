using Microsoft.Extensions.Logging;
using PRORC.Domain.Interfaces.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Infrastructure.Logging
{
    public class AuditLogger : IAuditLogger
    {
        private readonly ILogger<AuditLogger> _logger;

        public AuditLogger(ILogger<AuditLogger> logger)
        {
            _logger = logger;
        }

        public Task LogAsync(string action, string details)
        {
            _logger.LogInformation("AUDIT | Action: {Action} | Details: {Details} | Timestamp: {Timestamp}",
            action,
            details,
            DateTime.UtcNow);

            return Task.CompletedTask;
        }
    }
}
