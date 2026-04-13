
using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities.CharityModule;
using NetFoodia.Services.Specifications.CharitySpecifications;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.CommonResult;

namespace NetFoodia.Services
{
    public class AdminCharityService : IAdminCharityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminCharityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> VerifyCharityAsync(int charityId)
        {
            var repo = _unitOfWork.GetRepository<Charity>();

            var charity = await repo.GetByIdAsync(new CharityByIdSpec(charityId));
            if (charity is null)
                return Result.Fail(Error.NotFound("Charity.NotFound", "Charity not found"));

            if (charity.IsVerified && charity.MembershipStatus == CharityMembershipStatus.Active)
                return Result.OK();

            charity.IsVerified = true;
            charity.MembershipStatus = CharityMembershipStatus.Active;

            repo.Update(charity);
            await _unitOfWork.SaveChangesAsync();

            return Result.OK();
        }

        public async Task<Result> DeactivateCharityAsync(int charityId)
        {
            var repo = _unitOfWork.GetRepository<Charity>();

            var charity = await repo.GetByIdAsync(new CharityByIdSpec(charityId));
            if (charity is null)
                return Result.Fail(Error.NotFound("Charity.NotFound", "Charity not found"));

            if (!charity.IsVerified)
                return Result.Fail(Error.Validation("Charity.NotVerified", "Charity must be verified first"));

            if (charity.MembershipStatus == CharityMembershipStatus.Suspended)
                return Result.OK();

            charity.MembershipStatus = CharityMembershipStatus.Suspended;

            repo.Update(charity);
            await _unitOfWork.SaveChangesAsync();

            return Result.OK();
        }

        public async Task<Result> ReactivateCharityAsync(int charityId)
        {
            var repo = _unitOfWork.GetRepository<Charity>();

            var charity = await repo.GetByIdAsync(new CharityByIdSpec(charityId));
            if (charity is null)
                return Result.Fail(Error.NotFound("Charity.NotFound", "Charity not found"));

            if (!charity.IsVerified)
                return Result.Fail(Error.Validation("Charity.NotVerified", "Charity must be verified first"));

            if (charity.MembershipStatus == CharityMembershipStatus.Active)
                return Result.OK();

            charity.MembershipStatus = CharityMembershipStatus.Active;

            repo.Update(charity);
            await _unitOfWork.SaveChangesAsync();

            return Result.OK();
        }
    }
}