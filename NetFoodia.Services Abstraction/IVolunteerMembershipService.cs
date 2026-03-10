using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.MembershipDTOs;

namespace NetFoodia.Services_Abstraction
{
    public interface IVolunteerMembershipService
    {
        Task<Result<bool>> VolunteerApplyToCharity(string volunteerId, int charityId);
        Task<Result<VolunteerMembershipDTO>> GetActiveMembership(string volunteerId);
        Task<Result<IEnumerable<ListVolunteerMembershipDTO>>> CharityAdminListPendingApplications(int charityId);
        Task<Result<bool>> CharityAdminApproveVolunteer(int membershipId);
        Task<Result<bool>> CharityAdminRejectVolunteer(int membershipId, string reason);
        Task<Result<IEnumerable<ListVolunteerMembershipDTO>>> CharityAdminListMemberShips(int charityId);
        Task<Result<bool>> CharityAdminSuspendVolunteer(int membershipId, string reason);
        Task<Result<bool>> CharityAdminReactivateVolunteer(int membershipId);
    }
}
