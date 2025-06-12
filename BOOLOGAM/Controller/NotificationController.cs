using BOOLOG.Application.Dto.PropertyHubDto;
using BOOLOG.Application.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BOOLOG.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("send-to-all")]
        public async Task<IActionResult> SendToAll([FromForm] NotificationDto dto)
        {
            await _notificationService.SendToAllAsync(dto.Title, dto.Message);
            return Ok("Notification sent to all users.");
        }

        [HttpPost("send-to-user/{userId}")]
        public async Task<IActionResult> SendToUser(string userId,[FromForm] NotificationDto dto)
        {
            await _notificationService.SendToUserAsync(userId, dto.Title, dto.Message);
            return Ok($"Notification sent to user {userId}.");
        }

        [HttpPost("send-to-users")]
        public async Task<IActionResult> SendToUsers([FromForm] MultiUserNotificationDto dto)
        {
            await _notificationService.SendToUsersAsync(dto.UserIds, dto.Title, dto.Message);
            return Ok("Notification sent to selected users.");
        }

        [HttpPost("property-sold/{userId}")]
        public async Task<IActionResult> NotifyPropertySold(Guid userId, [FromQuery] string propertyTitle)
        {
            await _notificationService.NotifyPropertySoldAsync(userId, propertyTitle);
            return Ok("Property sold notification sent.");
        }
    }

}
