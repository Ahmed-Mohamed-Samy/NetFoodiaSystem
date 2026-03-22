using Microsoft.AspNetCore.SignalR;
using NetFoodia.Services.Hubs;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.CommonResult;


namespace NetFoodia.Services
{
    public class RealtimeNotificationService : IRealtimeNotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public RealtimeNotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task<Result> SendToUserAsync(string userId, object payload)
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", payload);
            return Result.OK();
        }
    }
}
