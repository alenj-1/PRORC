using Microsoft.Extensions.Logging;
using PRORC.Application.DTOs.Notifications;
using PRORC.Application.Interfaces;
using PRORC.Domain.Entities.Notifications;
using PRORC.Domain.Interfaces.Integrations;
using PRORC.Domain.Interfaces.Logging;
using PRORC.Domain.Interfaces.Repositories;

namespace PRORC.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationSender _notificationSender;
        private readonly IAuditLogger _auditLogger;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            INotificationRepository notificationRepository,
            IUserRepository userRepository,
            INotificationSender notificationSender,
            IAuditLogger auditLogger,
            ILogger<NotificationService> logger)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _notificationSender = notificationSender;
            _auditLogger = auditLogger;
            _logger = logger;
        }

        public async Task<NotificationDto> CreateAndSendAsync(int userId, string message, string type)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId)
                    ?? throw new KeyNotFoundException("User not found.");

                var notification = Notification.Create(userId, message, type);

                var created = await _notificationRepository.AddAsync(notification);

                // Si falla el correo, la operación principal no falla.  La notificación queda guardada igualmente.
                try
                {
                    await _notificationSender.SendAsync(
                        user.Email,
                        $"ChefCheck's - {type}",
                        message);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Notification email could not be sent to user {UserId}.", userId);
                }

                await TryWriteAuditAsync(
                    userId,
                    "CreateNotification",
                    "Notification",
                    created.Id,
                    $"Notification created with type {created.Type}.");

                return MapNotification(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating notification for user {UserId}.", userId);
                throw;
            }
        }

        public async Task<IEnumerable<NotificationDto>> GetByUserIdAsync(int userId)
        {
            try
            {
                var notifications = await _notificationRepository.GetByUserIdAsync(userId);
                return notifications.Select(MapNotification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting notifications for user {UserId}.", userId);
                throw;
            }
        }

        public async Task<IEnumerable<NotificationDto>> GetUnreadByUserIdAsync(int userId)
        {
            try
            {
                var notifications = await _notificationRepository.GetUnreadByUserIdAsync(userId);
                return notifications.Select(MapNotification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting unread notifications for user {UserId}.", userId);
                throw;
            }
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            try
            {
                var notification = await _notificationRepository.GetByIdAsync(notificationId)
                    ?? throw new KeyNotFoundException("Notification not found.");

                notification.MarkAsRead();

                await _notificationRepository.UpdateAsync(notification);

                await TryWriteAuditAsync(
                    notification.UserId,
                    "MarkNotificationAsRead",
                    "Notification",
                    notification.Id,
                    "Notification marked as read.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking notification {NotificationId} as read.", notificationId);
                throw;
            }
        }

        private NotificationDto MapNotification(Notification notification)
        {
            return new NotificationDto
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Message = notification.Message,
                Type = notification.Type,
                Read = notification.Read,
                SentAt = notification.SentAt
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
                _logger.LogWarning(ex, "Audit logging failed in NotificationService.");
            }
        }
    }
}