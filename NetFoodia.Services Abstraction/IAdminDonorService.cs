using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.ProfileDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetFoodia.Services_Abstraction
{
    public interface IAdminDonorService
    {
        Task<Result<IReadOnlyList<DonorDto>>> GetUnverifiedBusinessDonorsAsync();
        Task<Result> VerifyDonorAsync(string donorId);
        Task<Result> UnverifyDonorAsync(string donorId);
    }
}
