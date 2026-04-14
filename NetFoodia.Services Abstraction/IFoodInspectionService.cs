using NetFoodia.Shared;
using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.InspectionDTOs;

namespace NetFoodia.Services_Abstraction
{
    public interface IFoodInspectionService
    {
        Task<Result<bool>> CreateOrUpdateFromAI(int donationId);
        Task<Result<bool>> UpdateManual(int inspectionId, UpdateInspectionDTO dto, string adminId);
        Task<Result<FoodInspectionDTO?>> GetByDonation(int donationId);
        Task<Result<IEnumerable<FoodInspectionDTO>>> GetSuspiciousInspections();
        Task<Result<PaginatedResult<FoodInspectionDTO>>> GetAllInspectionsPaginated(int pageIndex, int pageSize, string? status);
    }
}
