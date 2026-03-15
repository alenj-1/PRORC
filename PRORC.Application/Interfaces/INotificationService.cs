using PRORC.Application.DTOs.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Application.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetByUserAsync(int userId);
        Task<List<NotificationDto>> GetUnreadByUserAsync(int userId);
        Task<int> CountUnreadAsync(int userId);
        Task MarkAsReadAsync(int notificationId);
    }
}
