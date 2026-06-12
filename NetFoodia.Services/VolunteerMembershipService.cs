using AutoMapper;
using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities.CharityModule;
using NetFoodia.Domain.Entities.MembershipModule;
using NetFoodia.Domain.Entities.ProfileModule;
using NetFoodia.Services.Specifications.MembershipSpecifications;
using Microsoft.AspNetCore.Identity;
using NetFoodia.Domain.Entities.IdentityModule;
using NetFoodia.Services.Specifications.ProfileSpecifications;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.MembershipDTOs;
using MembershipStatus = NetFoodia.Domain.Entities.MembershipModule.MembershipStatus;


namespace NetFoodia.Services
{
    public class VolunteerMembershipService : IVolunteerMembershipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;

        public VolunteerMembershipService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
            _userManager = userManager;
        }

        public async Task<Result<bool>> CharityAdminApproveVolunteer(int membershipId)
        {
            var membershipRepo = _unitOfWork.GetRepository<VolunteerMembership>();
            var membership = await membershipRepo.GetByIdAsync(membershipId);

            if (membership is null)
                return Error.NotFound("Membership.NotFound", "Membership not found");

            if (membership.Status != MembershipStatus.Pending)
                return Error.Validation("Membership.InvalidState", "Only Pending membership can be approved");

            membership.Status = MembershipStatus.Approved;
            var result = await _unitOfWork.SaveChangesAsync() > 0;

            if (result)
            {
                var volunteerUser = await _userManager.FindByIdAsync(membership.VolunteerId);
                if (volunteerUser != null && !string.IsNullOrEmpty(volunteerUser.Email))
                {
                    // To get charity name, we need to load it
                    var charityRepo = _unitOfWork.GetRepository<Charity>();
                    var charity = await charityRepo.GetByIdAsync(membership.CharityId);
                    var charityName = charity?.OrganizationName ?? "the charity";

                    string body = $$"""
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
</head>
<body style="margin: 0; padding: 0; background-color: #f6f4ee; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;">
    <div style="background-color: #f6f4ee; padding: 40px 20px; width: 100%; box-sizing: border-box;">
        
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px; background-color: #ffffff; border-radius: 16px; overflow: hidden; box-shadow: 0 4px 10px rgba(0,0,0,0.03);">
            
            <tr>
                <td style="padding: 30px 40px; text-align: center; border-bottom: 1px solid #f0f0f0;">
                    <span style="color: #cda25b; font-size: 20px; font-weight: 800; letter-spacing: 3px;">&mdash; NETFOODIA</span>
                </td>
            </tr>
            
            <tr>
                <td style="padding: 40px;">
                    <h2 style="margin: 0 0 20px 0; color: #1a1a1a; font-size: 26px;">Volunteer Membership Approved</h2>
                    <p style="margin: 0 0 25px 0; color: #6b7280; font-size: 16px; line-height: 1.6;">
                        Great news! Your volunteer membership for {{charityName}} has been approved. You are now part of the mission to deliver impact and can start accepting pickup tasks.
                    </p>
                    
                    <div style="text-align: center; margin: 35px 0;">
                        <a href="https://graduation-project-dun-five.vercel.app/volunteer/tasks" style="background-color: #1a1a1a; color: #ffffff; padding: 15px 40px; border-radius: 12px; text-decoration: none; display: inline-block; font-weight: bold;">View Pickup Tasks</a>
                    </div>
                </td>
            </tr>
            
            <tr>
                <td style="padding: 24px 40px; background-color: #faf9f6; text-align: center; border-top: 1px solid #f0f0f0;">
                    <p style="margin: 0; color: #9ca3af; font-size: 12px; line-height: 1.5;">
                        &copy; 2026 NetFoodia Workspace. All rights reserved.<br>
                        Deliver Impact.
                    </p>
                </td>
            </tr>
            
        </table>
    </div>
</body>
</html>
""";
                    await _emailService.SendEmailAsync(volunteerUser.Email, "Volunteer Membership Approved", body);
                }
            }

            return result;
        }

        public async Task<Result<IEnumerable<ListVolunteerMembershipDTO>>> CharityAdminListMemberShips(int charityId)
        {


            if (!await CharityIsVerfied(charityId))
                return Error.NotFound("Charity.NotFound", $"Charity With Id {charityId} not found Or Not Verfied");

            var repo = _unitOfWork.GetRepository<VolunteerMembership>();
            var spec = new CharityMembershipForVolunteerSpecification(charityId);


            var approvedMemberships = await repo.GetAllAsync(spec);

            if (approvedMemberships is null || !approvedMemberships.Any())
                return Error.NotFound("Memberships.NotFound", $"No memberships found for charity Id {charityId}");


            var approvedMembershipsDTO = _mapper.Map<IEnumerable<ListVolunteerMembershipDTO>>(approvedMemberships);
            return Result<IEnumerable<ListVolunteerMembershipDTO>>.OK(approvedMembershipsDTO);
        }

        public async Task<Result<IEnumerable<ListVolunteerMembershipDTO>>> CharityAdminListPendingApplications(int charityId)
        {

            if (!await CharityIsVerfied(charityId))
                return Error.NotFound("Charity.NotFound", $"Charity With Id {charityId} not found Or Not Verfied");

            var repo = _unitOfWork.GetRepository<VolunteerMembership>();
            var spec = new PendingVolunteersSpecification(charityId);

            var pendingMemberships = await repo.GetAllAsync(spec);

            if (pendingMemberships is null || !pendingMemberships.Any())
                return Error.NotFound("Memberships.NotFound", $"No pending memberships found for charity Id {charityId}");


            var pendingMembershipsDTO = _mapper.Map<IEnumerable<ListVolunteerMembershipDTO>>(pendingMemberships);
            return Result<IEnumerable<ListVolunteerMembershipDTO>>.OK(pendingMembershipsDTO);


        }

        public async Task<Result<bool>> CharityAdminReactivateVolunteer(int membershipId)
        {
            var membershipRepo = _unitOfWork.GetRepository<VolunteerMembership>();
            var membership = await membershipRepo.GetByIdAsync(membershipId);

            if (membership is null)
                return Error.NotFound("Membership.NotFound", "Membership not found");

            if (membership.Status != MembershipStatus.Suspended)
                return Error.Validation("Membership.InvalidState", "Only Suspended membership can be reactivated");

            membership.Status = MembershipStatus.Approved;
            membership.SuspendReason = null;
            var result = await _unitOfWork.SaveChangesAsync() > 0;

            return result;
        }

        public async Task<Result<bool>> CharityAdminRejectVolunteer(int membershipId, string reason)
        {
            var membershipRepo = _unitOfWork.GetRepository<VolunteerMembership>();
            var membership = await membershipRepo.GetByIdAsync(membershipId);

            if (membership is null)
                return Error.NotFound("Membership.NotFound", "Membership not found");

            if (membership.Status != MembershipStatus.Pending)
                return Error.Validation("Membership.InvalidState", "Only Pending membership can be rejected");

            membership.Status = MembershipStatus.Rejected;
            membership.RejectionReason = reason;
            var result = await _unitOfWork.SaveChangesAsync() > 0;

            return result;
        }

        public async Task<Result<bool>> CharityAdminSuspendVolunteer(int membershipId, string reason)
        {
            var membershipRepo = _unitOfWork.GetRepository<VolunteerMembership>();
            var membership = await membershipRepo.GetByIdAsync(membershipId);

            if (membership is null)
                return Error.NotFound("Membership.NotFound", "Membership not found");

            if (membership.Status != MembershipStatus.Approved)
                return Error.Validation("Membership.InvalidState", "Only Approved membership can be Suspended");

            membership.Status = MembershipStatus.Suspended;
            membership.SuspendReason = reason;


            var result = await _unitOfWork.SaveChangesAsync() > 0;

            return result;
        }

        public async Task<Result<VolunteerMembershipDTO>> GetActiveMembership(string volunteerId)
        {

            var membershipRepo = _unitOfWork.GetRepository<VolunteerMembership>();
            var spec = new VolunteerMembershipStatusSpecification(volunteerId);


            var membership = await membershipRepo.FirstOrDefaultAsync(spec);

            if (membership is null)
                return Error.NotFound("Membership.NotFound", "No Active Membership");


            var membershipDTO = _mapper.Map<VolunteerMembershipDTO>(membership);


            return membershipDTO;

        }

        public async Task<Result<bool>> VolunteerApplyToCharity(string volunteerId, int charityId)
        {
            var volunteerSpec = new VolunteerProfileSpecification(volunteerId);
            var volunteer = await _unitOfWork.GetRepository<VolunteerProfile>().GetByIdAsync(volunteerSpec);
            var membershipRepo = _unitOfWork.GetRepository<VolunteerMembership>();


            if (volunteer is null)
                return Error.NotFound("VolunteerProfile.NotFound", $"Volunteer profile for user {volunteerId} not found.");

            if (!await CharityIsVerfied(charityId))
                return Error.NotFound("Charity.NotFound", $"Charity With Id {charityId} not found Or Not Verfied");

            if (await HasActiveMembership(volunteerId, membershipRepo))
                return Error.Failure("Membership.Fail", "You already have an active membership or pending request");

            var membership = new VolunteerMembership()
            {
                CharityId = charityId,
                VolunteerId = volunteerId,
                Status = MembershipStatus.Pending
            };

            await membershipRepo.AddAsync(membership);

            var result = await _unitOfWork.SaveChangesAsync() > 0;

            if (!result)
                return Error.Failure("Membership.CreateFailed", "Failed to submit application");

            return result;

        }


        #region Helper Methods

        async Task<bool> HasActiveMembership(string volunteerId, IGenericRepository<VolunteerMembership> repo)
        {

            var spec = new ActiveMembershipForVolunteerSpecification(volunteerId);

            return await repo.AnyAsync(spec);
        }

        async Task<bool> CharityIsVerfied(int charityId)
        {
            var charity = await _unitOfWork.GetRepository<Charity>().GetByIdAsync(charityId);

            if (charity is null || !charity.IsVerified)
                return false;
            return true;
        }
        #endregion
    }
}
