using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace NetFoodia.Services.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
    }
}