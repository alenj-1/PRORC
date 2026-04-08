namespace PRORC.Domain.Interfaces.Integrations
{
    public interface INotificationSender
    {
        Task SendAsync(string to, string subject, string message);
    }
}