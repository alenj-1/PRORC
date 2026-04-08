namespace PRORC.Domain.Interfaces.Logging
{
    public interface IAuditLogger
    {
        Task LogAsync(int? userId, string action, string entityName, int entityId, string details);
    }
}