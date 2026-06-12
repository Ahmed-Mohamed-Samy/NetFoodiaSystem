
using AutoMapper;
using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities.CharityModule;
using NetFoodia.Services.Specifications.CharitySpecifications;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared;
using NetFoodia.Shared.CharityDTOs;
using NetFoodia.Shared.CommonResult;
using CharityMembershipStatus = NetFoodia.Domain.Entities.CharityModule.CharityMembershipStatus;

namespace NetFoodia.Services
{
    public class AdminCharityService : IAdminCharityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public AdminCharityService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
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
                    <h2 style="margin: 0 0 20px 0; color: #1a1a1a; font-size: 26px;">Charity Verification Approved</h2>
                    <p style="margin: 0 0 25px 0; color: #6b7280; font-size: 16px; line-height: 1.6;">
                        Congratulations! Your charity organization has been successfully verified and approved by the NetFoodia administration. You can now log in to your workspace to manage donations.
                    </p>
                    
                    <div style="text-align: center; margin: 35px 0;">
                        <a href="https://graduation-project-dun-five.vercel.app/login" style="background-color: #1a1a1a; color: #ffffff; padding: 15px 40px; border-radius: 12px; text-decoration: none; display: inline-block; font-weight: bold;">Login to Workspace</a>
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

            if (charity.AdminProfile?.User?.Email != null)
            {
                await _emailService.SendEmailAsync(charity.AdminProfile.User.Email, "Charity Verification Approved", body);
            }

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

        public async Task<Result<PaginatedResult<CharityListItemDTO>>> ListCharitiesAsync(PaginationParams pagination, string? search)
        {
            var charityRepo = _unitOfWork.GetRepository<Charity>();

            var listSpec = new CharitiesListSpec(search, pagination.PageIndex, pagination.PageSize);
            var countSpec = new CharitiesCountSpec(search);

            var charities = await charityRepo.GetAllAsync(listSpec);
            var total = await charityRepo.CountAsync(countSpec);

            var items = _mapper.Map<List<CharityListItemDTO>>(charities);

            return new PaginatedResult<CharityListItemDTO>(pagination.PageIndex, pagination.PageSize, total, items);
        }
    }
}