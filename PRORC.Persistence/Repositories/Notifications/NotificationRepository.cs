using Microsoft.EntityFrameworkCore;
using PRORC.Domain.Entities.Notifications;
using PRORC.Domain.Interfaces.Repositories;
using PRORC.Persistence.Base;
using PRORC.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Persistence.Repositories.Notifications
{
    public class NotificationRepository : BaseRepository<Notification, int>, INotificationRepository
    {
        public NotificationRepository(PRORCContext context) : base(context)
        {
        }

        public async Task<List<Notification>> GetNotificationsByUserAsync(int userId)
        {
            return await _dbSet
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Notification>> GetUnreadNotificationsAsync(int userId)
        {
            return await _dbSet
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Notification>> GetReadNotificationsAsync(int userId)
        {
            return await _dbSet
                .Where(n => n.UserId == userId && n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> CountUnreadNotificationsAsync(int userId)
        {
            return await _dbSet
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }

        public async Task<List<Notification>> GetLatestNotificationsAsync(int userId, int count)
        {
            return await _dbSet
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _dbSet.FirstOrDefaultAsync(n => n.Id == notificationId);

            if (notification is null)
                return;

            notification.IsRead = true;
            _dbSet.Update(notification);
            await _context.SaveChangesAsync();
        }
    }
}