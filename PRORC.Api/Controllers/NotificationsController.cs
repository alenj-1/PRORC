using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRORC.Api.Security;
using PRORC.Application.DTOs.Notifications;
using PRORC.Application.Interfaces;

namespace PRORC.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController(INotificationService notificationService) : ControllerBase
    {
        private readonly INotificationService _notificationService = notificationService;

        // GET que permite devolver todas las notificaciones de un usuario
        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<List<NotificationDto>>> GetByUser(int userId)
        {
            if (!CanAccessUserData(userId))
            {
                return Forbid();
            }

            var notifications = await _notificationService.GetByUserAsync(userId);
            return Ok(notifications);
        }


        // GET que permite devolver las notificaciones no leídas de un usuario
        [HttpGet("user/{userId:int}/unread")]
        public async Task<ActionResult<List<NotificationDto>>> GetUnreadByUser(int userId)
        {
            if (!CanAccessUserData(userId))
            {
                return Forbid();
            }

            var notifications = await _notificationService.GetUnreadByUserAsync(userId);
            return Ok(notifications);
        }


        // GET que permite contar cuántas notificaciones no leídas tiene un usuario
        [HttpGet("user/{userId:int}/unread/count")]
        public async Task<ActionResult<int>> CountUnread(int userId)
        {
            if (!CanAccessUserData(userId))
            {
                return Forbid();
            }

            var count = await _notificationService.CountUnreadAsync(userId);
            return Ok(count);
        }


        // PATCH que permite marcar una notificación como leída
        [HttpPatch("{notificationId:int}/read")]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            await _notificationService.MarkAsReadAsync(notificationId);
            return NoContent();
        }


        // Método para evitar que un usuario que no tenga rol de SystemAdmin consulte los datos de otro usuario
        private bool CanAccessUserData(int userId)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (role == "SystemAdmin")
            {
                return true;
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return false;
            }

            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                return false;
            }

            return currentUserId == userId;
        }
    }
}