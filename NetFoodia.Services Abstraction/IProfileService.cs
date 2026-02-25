using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.ProfileDTOs;

namespace NetFoodia.Services_Abstraction
{
    public interface IProfileService
    {
        Task<Result<MyProfileDTO>> GetMyProfileAsync(string userId, string role);
        Task<Result> UpdateMyProfileAsync(string userId, UpdateMyProfileDTO profileDTO);

        Task<Result> UpsertDonorAsync(string userId, UpsertDonorProfileDTO upsertDonorProfile);
        Task<Result> UpsertVolunteerAsync(string userId, UpsertVolunteerProfileDTO upsertVolunteerProfile);
        Task<Result> SetVolunteerStatusAsync(string userId, SetVolunteerStatusDTO status);

    }
}
