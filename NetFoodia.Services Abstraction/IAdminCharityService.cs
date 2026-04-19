using NetFoodia.Shared;
using NetFoodia.Shared.CharityDTOs;
using NetFoodia.Shared.CommonResult;

namespace NetFoodia.Services_Abstraction
{
    public interface IAdminCharityService
    {
        Task<Result> VerifyCharityAsync(int charityId);
        Task<Result> DeactivateCharityAsync(int charityId);
        Task<Result> ReactivateCharityAsync(int charityId);
        Task<Result<PaginatedResult<CharityListItemDTO>>> ListCharitiesAsync(PaginationParams pagination, string? search);
    }
}
