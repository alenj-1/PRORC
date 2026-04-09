using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRORC.Domain.Entities.Notifications;
using PRORC.Domain.Interfaces.Repositories;
using PRORC.Persistence.Base;
using PRORC.Persistence.Context;

namespace PRORC.Persistence.Repositories.Notifications
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(PRORCContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory) { }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(int userId)
        {
            try
            {
                return await _context.Notifications
                    .AsNoTracking()
                    .Where(n => n.UserId == userId)
                    .OrderByDescending(n => n.SentAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting notifications by UserId {UserId}.", userId);
                throw;
            }
        }

        public async Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(int userId)
        {
            try
            {
                return await _context.Notifications
                    .AsNoTracking()
                    .Where(n => n.UserId == userId && !n.Read)
                    .OrderByDescending(n => n.SentAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting unread notifications by UserId {UserId}.", userId);
                throw;
            }
        }

        public async Task<int> CountUnreadByUserIdAsync(int userId)
        {
            try
            {
                return await _context.Notifications
                    .CountAsync(n => n.UserId == userId && !n.Read);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting unread notifications by UserId {UserId}.", userId);
                throw;
            }
        }
    }
}