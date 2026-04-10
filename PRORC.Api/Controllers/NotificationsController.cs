using Microsoft.AspNetCore.Mvc;
using PRORC.Application.Interfaces;

namespace PRORC.Api.Controllers
{
    [ApiController]
    [Route("api/notifications")]

    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }


        // POST que permite crear y enviar una notificación
        [HttpPost]
        public async Task<IActionResult> CreateAndSend([FromBody] CreateNotificationApiRequest request)
        {
            var result = await _notificationService.CreateAndSendAsync(request.UserId, request.Message, request.Type);
            return Ok(result);
        }


        // GET que permite obtener todas las notificaciones de un usuario
        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var result = await _notificationService.GetByUserIdAsync(userId);
            return Ok(result);
        }


        // GET que permite obtener las notificaciones no leídas de un usuario
        [HttpGet("user/{userId:int}/unread")]
        public async Task<IActionResult> GetUnreadByUserId(int userId)
        {
            var result = await _notificationService.GetUnreadByUserIdAsync(userId);
            return Ok(result);
        }

        // PATCH que permite marcar una notificación como leída
        [HttpPatch("{notificationId:int}/read")]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            await _notificationService.MarkAsReadAsync(notificationId);
            return Ok(new { message = "Notification marked as read." });
        }


        // Clase simple
        public class CreateNotificationApiRequest
        {
            public int UserId { get; set; }
            public string Message { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
        }
    }
}