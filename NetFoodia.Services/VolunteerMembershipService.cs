using AutoMapper;
using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities.CharityModule;
using NetFoodia.Domain.Entities.MembershipModule;
using NetFoodia.Domain.Entities.ProfileModule;
using NetFoodia.Services.Specifications.MembershipSpecifications;
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

        public VolunteerMembershipService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
