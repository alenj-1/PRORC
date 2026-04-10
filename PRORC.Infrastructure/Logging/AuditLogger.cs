using Microsoft.Extensions.Logging;
using PRORC.Domain.Interfaces.Logging;

namespace PRORC.Infrastructure.Logging
{
    public class AuditLogger : IAuditLogger
    {
        private readonly ILogger<AuditLogger> _logger;

        public AuditLogger(ILogger<AuditLogger> logger)
        {
            _logger = logger;
        }

        // Registra una acción importante del sistema
        public async Task LogAsync(int? userId, string action, string entityName, int entityId, string details)
        {
            try
            {
                // Simula async para mantener consistencia con la interfaz
                await Task.CompletedTask;

                _logger.LogInformation(
                    "AUDIT LOG | UserId: {UserId}, Action: {Action}, Entity: {EntityName}, EntityId: {EntityId}, Details: {Details}",
                    userId,
                    action,
                    entityName,
                    entityId,
                    details);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while writing audit log.");
                throw;
            }
        }
    }
}