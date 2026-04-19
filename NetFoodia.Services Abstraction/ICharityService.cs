using NetFoodia.Shared;
using NetFoodia.Shared.CharityDTOs;
using NetFoodia.Shared.CommonResult;

namespace NetFoodia.Services_Abstraction
{
    public interface ICharityService
    {
        Task<Result<CharityDetailsDTO>> CreateMyCharityAsync(string userId, CreateMyCharityDTO dto);
        Task<Result> UpdateCharityInfoAsync(string userId, UpdateCharityInfoDTO dto);
        Task<Result> UpdateCharityLocationAsync(string userId, UpdateCharityLocationDTO dto);

        Task<Result<CharityDetailsDTO>> GetCharityDetailsAsync(int charityId);
        Task<Result<PaginatedResult<CharityListItemDTO>>> ListActiveAndVerfiedCharitiesAsync(PaginationParams pagination, string? search);
        Task<Result<int>> GetCharityId(string charityAdminId);
    }
}
