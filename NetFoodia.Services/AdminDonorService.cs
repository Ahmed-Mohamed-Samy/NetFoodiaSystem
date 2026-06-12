using AutoMapper;
using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities.ProfileModule;
using NetFoodia.Services.Specifications.ProfileSpecifications;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.ProfileDTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NetFoodia.Domain.Entities.IdentityModule;

namespace NetFoodia.Services
{
    public class AdminDonorService : IAdminDonorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminDonorService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
            _userManager = userManager;
        }

        public async Task<Result<IReadOnlyList<DonorDto>>> GetUnverifiedBusinessDonorsAsync()
        {
            var repo = _unitOfWork.GetRepository<DonorProfile>();
            var spec = new UnverifiedBusinessDonorsSpecification();
            var donors = await repo.GetAllAsync(spec);

            var dtos = _mapper.Map<IReadOnlyList<DonorDto>>(donors);

            return Result<IReadOnlyList<DonorDto>>.OK(dtos);
        }

        public async Task<Result> VerifyDonorAsync(string donorId)
        {
            var repo = _unitOfWork.GetRepository<DonorProfile>();
            var donor = await repo.FirstOrDefaultAsync(new DonorProfileSpecification(donorId));

            if (donor == null)
            {
                return Result.Fail(Error.NotFound("Donor.NotFound", $"Donor profile for user {donorId} not found"));
            }

            donor.IsVerified = true;
            repo.Update(donor);

            await _unitOfWork.SaveChangesAsync();

            var donorUser = await _userManager.FindByIdAsync(donorId);
            if (donorUser != null && !string.IsNullOrEmpty(donorUser.Email))
            {
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
                    <h2 style="margin: 0 0 20px 0; color: #1a1a1a; font-size: 26px;">Donor Account Verified</h2>
                    <p style="margin: 0 0 25px 0; color: #6b7280; font-size: 16px; line-height: 1.6;">
                        Great news! Your donor account has been successfully verified by the NetFoodia administration. You can now start donating food, managing your donations, and delivering impact.
                    </p>
                    
                    <div style="text-align: center; margin: 35px 0;">
                        <a href="https://graduation-project-dun-five.vercel.app/donor/dashboard" style="background-color: #1a1a1a; color: #ffffff; padding: 15px 40px; border-radius: 12px; text-decoration: none; display: inline-block; font-weight: bold;">Donate Now</a>
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
                await _emailService.SendEmailAsync(donorUser.Email, "Donor Account Verified", body);
            }

            return Result.OK();
        }

        public async Task<Result> UnverifyDonorAsync(string donorId)
        {
            var repo = _unitOfWork.GetRepository<DonorProfile>();
            var donor = await repo.FirstOrDefaultAsync(new DonorProfileSpecification(donorId));

            if (donor == null)
            {
                return Result.Fail(Error.NotFound("Donor.NotFound", $"Donor profile for user {donorId} not found"));
            }

            donor.IsVerified = false;
            repo.Update(donor);

            await _unitOfWork.SaveChangesAsync();
            return Result.OK();
        }
    }
}
