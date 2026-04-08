using PRORC.Domain.Entities.Notifications;

namespace PRORC.Domain.Interfaces.Repositories
{
    public interface INotificationRepository : IBaseRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(int userId);
        Task<int> CountUnreadByUserIdAsync(int userId);
    }
}