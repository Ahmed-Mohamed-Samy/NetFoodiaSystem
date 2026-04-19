using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.DashboardDTOs;

namespace NetFoodia.Services_Abstraction
{
    public interface IDashboardService
    {
        Task<Result<DashboardStatsDTO>> GetStatsAsync(int? charityId = null);
        Task<Result> SendRealTimeUpdate(int charityId);
    }
}
