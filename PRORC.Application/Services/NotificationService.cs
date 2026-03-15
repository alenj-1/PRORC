using PRORC.Application.DTOs.Notifications;
using PRORC.Application.Interfaces;
using PRORC.Domain.Entities.Notifications;
using PRORC.Domain.Interfaces.Repositories;

namespace PRORC.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<List<NotificationDto>> GetByUserAsync(int userId)
        {
            var notifications = await _notificationRepository.GetNotificationsByUserAsync(userId);
            return notifications.Select(Map).ToList();
        }

        public async Task<List<NotificationDto>> GetUnreadByUserAsync(int userId)
        {
            var notifications = await _notificationRepository.GetUnreadNotificationsAsync(userId);
            return notifications.Select(Map).ToList();
        }

        public Task<int> CountUnreadAsync(int userId)
        {
            return _notificationRepository.CountUnreadNotificationsAsync(userId);
        }

        public Task MarkAsReadAsync(int notificationId)
        {
            return _notificationRepository.MarkAsReadAsync(notificationId);
        }

        private static NotificationDto Map(Notification notification) => new()
        {
            Id = notification.Id,
            UserId = notification.UserId,
            Title = notification.Title,
            Message = notification.Message,
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt
        };
    }
}
