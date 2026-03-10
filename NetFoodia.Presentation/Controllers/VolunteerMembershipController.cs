using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.MembershipDTOs;

namespace NetFoodia.Presentation.Controllers
{
    public class VolunteerMembershipController : ApiBaseController
    {
        private readonly IVolunteerMembershipService _membershipService;

        public VolunteerMembershipController(IVolunteerMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        [Authorize(Roles = "Volunteer")]
        [HttpPost("apply/{charityId}")]
        public async Task<ActionResult<bool>> ApplyToCharity(int charityId)
        {
            var volunteerId = GetUserIdFromToken();

            var result = await _membershipService.VolunteerApplyToCharity(volunteerId, charityId);

            return HandleResult(result);
        }

        [Authorize(Roles = "CharityAdmin")]
        [HttpGet("{charityId}/pending")]
        public async Task<ActionResult<IEnumerable<ListVolunteerMembershipDTO>>> GetPendingApplications(int charityId)
        {
            var result = await _membershipService.CharityAdminListPendingApplications(charityId);

            return HandleResult(result);
        }

        [Authorize(Roles = "CharityAdmin")]
        [HttpGet("{charityId}/memberships")]
        public async Task<ActionResult<IEnumerable<ListVolunteerMembershipDTO>>> GetMemberships(int charityId)
        {
            var result = await _membershipService.CharityAdminListMemberShips(charityId);
            return HandleResult(result);
        }

        [Authorize(Roles = "CharityAdmin")]
        [HttpPost("approve/{membershipId}")]
        public async Task<ActionResult<bool>> ApproveVolunteer(int membershipId)
        {
            var result = await _membershipService.CharityAdminApproveVolunteer(membershipId);

            return HandleResult(result);
        }


        [Authorize(Roles = "CharityAdmin")]
        [HttpPost("reject/{membershipId}")]
        public async Task<ActionResult<bool>> RejectVolunteer(int membershipId, RejectVolunteerDTO rejectVolunteerDTO)
        {
            var result = await _membershipService.CharityAdminRejectVolunteer(membershipId, rejectVolunteerDTO.RejectionReason!);

            return HandleResult(result);
        }


        [Authorize(Roles = "CharityAdmin")]
        [HttpPost("suspend/{membershipId}")]
        public async Task<ActionResult<bool>> SuspendVolunteer(int membershipId, SuspendVolunteerDTO suspendVolunteerDTO)
        {
            var result = await _membershipService.CharityAdminSuspendVolunteer(membershipId, suspendVolunteerDTO.Reason!);

            return HandleResult(result);
        }


        [Authorize(Roles = "CharityAdmin")]
        [HttpPost("reactivate/{membershipId}")]
        public async Task<ActionResult<bool>> ReactivateVolunteer(int membershipId)
        {
            var result = await _membershipService.CharityAdminReactivateVolunteer(membershipId);

            return HandleResult(result);
        }


        [Authorize(Roles = "Volunteer")]
        [HttpGet("my-membership")]
        public async Task<ActionResult<VolunteerMembershipDTO>> GetMyMembership()
        {
            var volunteerId = GetUserIdFromToken();
            var result = await _membershipService.GetActiveMembership(volunteerId);

            return HandleResult(result);
        }
    }
}
