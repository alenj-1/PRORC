using PRORC.Domain.Entities.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Domain.Interfaces.Repositories
{
    public interface INotificationRepository : IBaseRepository<Notification, int>
    {
        Task<List<Notification>> GetNotificationsByUserAsync(int userId);
        Task<List<Notification>> GetUnreadNotificationsAsync(int userId);
        Task<List<Notification>> GetReadNotificationsAsync(int userId);
        Task<int> CountUnreadNotificationsAsync(int userId);
        Task<List<Notification>> GetLatestNotificationsAsync(int userId, int count);
        Task MarkAsReadAsync(int notificationId);
    }
}
