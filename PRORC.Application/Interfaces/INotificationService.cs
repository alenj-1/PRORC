using PRORC.Application.DTOs.Notifications;

namespace PRORC.Application.Interfaces
{
    public interface INotificationService
    {
        Task<NotificationDto> CreateAndSendAsync(int userId, string message, string type);
        Task<IEnumerable<NotificationDto>> GetByUserIdAsync(int userId);
        Task<IEnumerable<NotificationDto>> GetUnreadByUserIdAsync(int userId);
        Task MarkAsReadAsync(int notificationId);
    }
}