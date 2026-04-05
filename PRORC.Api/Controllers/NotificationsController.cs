using Microsoft.AspNetCore.Mvc;
using PRORC.Application.Interfaces;

namespace PRORC.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var result = await _notificationService.GetByUserAsync(userId);
            return Ok(result);
        }

        [HttpGet("user/{userId:int}/unread")]
        public async Task<IActionResult> GetUnreadByUser(int userId)
        {
            var result = await _notificationService.GetUnreadByUserAsync(userId);
            return Ok(result);
        }

        [HttpGet("user/{userId:int}/unread/count")]
        public async Task<IActionResult> CountUnread(int userId)
        {
            var result = await _notificationService.CountUnreadAsync(userId);
            return Ok(new { userId, unreadCount = result });
        }

        [HttpPut("{id:int}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _notificationService.MarkAsReadAsync(id);
            return Ok(new { message = "Notification marked as read." });
        }
    }
}
