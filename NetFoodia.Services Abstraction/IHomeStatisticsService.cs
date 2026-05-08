using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.HomeDTOs;
using System.Threading.Tasks;

namespace NetFoodia.Services_Abstraction
{
    public interface IHomeStatisticsService
    {
        Task<Result<HomeStatisticsDto>> GetImpactStatisticsAsync();
    }
}
