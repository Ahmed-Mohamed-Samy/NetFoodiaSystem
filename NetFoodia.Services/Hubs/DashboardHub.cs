using Microsoft.AspNetCore.SignalR;

namespace NetFoodia.Services.Hubs
{
    public class DashboardHub : Hub
    {

        public async Task JoinCharityDashboard(int charityId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Charity_{charityId}");
        }


        public async Task JoinSuperAdminDashboard()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "AdminGroup");
        }
    }
}
