using PRORC.Domain.Entities.Notifications;
using PRORC.Domain.Interfaces.Integrations;
using PRORC.Domain.Interfaces.Logging;
using PRORC.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Infrastructure.Integrations.Notifications
{
    public class EmailNotificationService : INotificationSender
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IAuditLogger _auditLogger;

        public EmailNotificationService(INotificationRepository notificationRepository, IAuditLogger auditLogger)
        {
            _notificationRepository = notificationRepository;
            _auditLogger = auditLogger;
        }

        public async Task SendAsync(int userId, string title, string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationRepository.AddAsync(notification);
            await _auditLogger.LogAsync("NotificationSent", $"Notification stored for user {userId} with title '{title}'");
        }
    }
}