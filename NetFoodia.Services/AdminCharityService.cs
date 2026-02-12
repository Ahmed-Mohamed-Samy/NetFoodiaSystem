using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities.CharityModule;
using NetFoodia.Services.Specifications.CharitySpecifications;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.CommonResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            if (charity.IsVerified)
                return Result.OK(); 

            charity.IsVerified = true;

            repo.Update(charity);
            await _unitOfWork.SaveChangesAsync();

            return Result.OK();
        }

        public async Task<Result> ActivateCharityAsync(int charityId)
        {
            var repo = _unitOfWork.GetRepository<Charity>();

            var charity = await repo.GetByIdAsync(new CharityByIdSpec(charityId));
            if (charity is null)
                return Result.Fail(Error.NotFound("Charity.NotFound", "Charity not found"));

            
            if (!charity.IsVerified)
                return Result.Fail(Error.Validation("Charity.NotVerified", "Charity must be verified before activation"));

            if (charity.MembershipStatus == CharityMembershipStatus.Active)
                return Result.OK(); 

            charity.MembershipStatus = CharityMembershipStatus.Active;

            repo.Update(charity);
            await _unitOfWork.SaveChangesAsync();

            return Result.OK();
        }
    }
}
