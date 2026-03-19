using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.NotificationDTOs;


namespace NetFoodia.Services_Abstraction
{
    public interface INotificationService
    {
        Task<Result> CreateNotificationAsync(
            string userId,
            string title,
            string message,
            int type,
            int? relatedTaskId = null,
            int? relatedDonationId = null);

        Task<Result<IEnumerable<NotificationDTO>>> GetMyNotificationsAsync(string userId);
        Task<Result<bool>> MarkAsReadAsync(string userId, int notificationId);
    }
}
