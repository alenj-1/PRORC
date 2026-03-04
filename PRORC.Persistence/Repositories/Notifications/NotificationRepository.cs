using PRORC.Domain.Entities.Menus;
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
    }
}