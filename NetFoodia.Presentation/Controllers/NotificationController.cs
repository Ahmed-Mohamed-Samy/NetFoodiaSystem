using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.NotificationDTOs;

namespace NetFoodia.Presentation.Controllers
{
    [Authorize]
    [Route("api/Notifications")]
    public class NotificationController : ApiBaseController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("My")]
        public async Task<ActionResult<IEnumerable<NotificationDTO>>> GetMyNotifications()
        {
            var userId = GetUserIdFromToken();
            var result = await _notificationService.GetMyNotificationsAsync(userId);
            return HandleResult(result);
        }

        [HttpPost("Read/{notificationId:int}")]
        public async Task<ActionResult<bool>> MarkAsRead(int notificationId)
        {
            var userId = GetUserIdFromToken();
            var result = await _notificationService.MarkAsReadAsync(userId, notificationId);
            return HandleResult(result);
        }
    }
}
