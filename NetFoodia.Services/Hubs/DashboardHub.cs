using Microsoft.AspNetCore.SignalR;

namespace NetFoodia.Services.Hubs
{
    public class DashboardHub : Hub
    {

        public async Task JoinCharityGroup(int charityId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Charity_{charityId}");
        }

        public async Task JoinSuperAdminGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SuperAdminGroup");
        }
    }
}
