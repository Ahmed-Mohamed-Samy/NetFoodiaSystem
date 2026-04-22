using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities.CharityModule;
using NetFoodia.Domain.Entities.IdentityModule;
using NetFoodia.Domain.Entities.ProfileModule;
using NetFoodia.Services.Specifications.ProfileSpecifications;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.ProfileDTOs;
using VolunteerStatus = NetFoodia.Domain.Entities.ProfileModule.VolunteerStatus;

namespace NetFoodia.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<Result<MyProfileDTO>> GetMyProfileAsync(string userId, string role)
        {
            var donorRepo = _unitOfWork.GetRepository<DonorProfile>();
            var volunteerRepo = _unitOfWork.GetRepository<VolunteerProfile>();
            var charityAdminRepo = _unitOfWork.GetRepository<CharityAdminProfile>();

            role = role.Trim();

            return role switch
            {
                "Donor" => await GetDonor(userId),
                "Volunteer" => await GetVolunteer(userId),
                "CharityAdmin" => await GetCharityAdmin(userId),
                _ => Error.BadRequest("Role.Invalid", $"Unsupported role: {role}")
            };



        }

        public Task<Result> UpdateMyProfileAsync(string userId, UpdateMyProfileDTO profileDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> UpsertDonorAsync(string userId, UpsertDonorProfileDTO dto)
        {
            var donorRepo = _unitOfWork.GetRepository<DonorProfile>();
            var spec = new DonorProfileSpecification(userId);

            var profile = await donorRepo.GetByIdAsync(spec);

            if (profile is null)
            {
                profile = _mapper.Map<DonorProfile>(dto);
                profile.UserId = userId;
                profile.IsVerified = false;
                profile.ReliabilityScore = 0m;
                var user = await _userManager.FindByIdAsync(userId);
                user!.IsCompleted = true;
                await donorRepo.AddAsync(profile);
            }
            else
            {
                _mapper.Map(dto, profile);
                donorRepo.Update(profile);
            }


            if (!profile.IsBusiness)
            {
                profile.BusinessType = null;
                profile.IsVerified = true;
            }

            await _unitOfWork.SaveChangesAsync();
            return Result.OK();
        }

        public async Task<Result> UpsertVolunteerAsync(string userId, UpsertVolunteerProfileDTO dto)
        {
            var repo = _unitOfWork.GetRepository<VolunteerProfile>();
            var spec = new VolunteerProfileSpecification(userId);

            var profile = await repo.GetByIdAsync(spec);

            if (profile is null)
            {
                profile = _mapper.Map<VolunteerProfile>(dto);

                profile.UserId = userId;
                profile.Status = VolunteerStatus.Offline;
                profile.LastActiveAt = DateTime.UtcNow;


                profile.Address ??= string.Empty;
                profile.IsVerified = true;
                var user = await _userManager.FindByIdAsync(userId);
                user!.IsCompleted = true;
                await repo.AddAsync(profile);
            }
            else
            {
                _mapper.Map(dto, profile);
                profile.LastActiveAt = DateTime.UtcNow;
                repo.Update(profile);

            }

            await _unitOfWork.SaveChangesAsync();
            return Result.OK();
        }

        public async Task<Result> SetVolunteerStatusAsync(string userId, SetVolunteerStatusDTO status)
        {
            var repo = _unitOfWork.GetRepository<VolunteerProfile>();

            var profile = await repo.GetByIdAsync(new VolunteerProfileSpecification(userId));
            if (profile is null)
                return Result.Fail(Error.NotFound("VolunteerProfile.NotFound", $"Volunteer profile for user {userId} not found."));

            profile.Status = (VolunteerStatus)(int)status.Status;
            profile.LastActiveAt = DateTime.UtcNow;
            repo.Update(profile);
            await _unitOfWork.SaveChangesAsync();
            return Result.OK();
        }


        #region Helper Methods
        private async Task<Result<MyProfileDTO>> GetDonor(string userId)
        {
            var spec = new DonorProfileSpecification(userId);
            var profile = await _unitOfWork.GetRepository<DonorProfile>().GetByIdAsync(spec);

            if (profile is null) return Error.NotFound("User.NotFound", $"User with Id {userId} Is Not Found");

            return _mapper.Map<MyProfileDTO>(profile);
        }


        private async Task<Result<MyProfileDTO>> GetVolunteer(string userId)
        {
            var spec = new VolunteerProfileSpecification(userId);
            var profile = await _unitOfWork.GetRepository<VolunteerProfile>().GetByIdAsync(spec);

            if (profile is null)
                return Error.NotFound("User.NotFound", $"User with Id {userId} Is Not Found");

            return _mapper.Map<MyProfileDTO>(profile);
        }

        private async Task<Result<MyProfileDTO>> GetCharityAdmin(string userId)
        {
            var spec = new CharityAdminProfileSpec(userId);
            var profile = await _unitOfWork.GetRepository<CharityAdminProfile>().GetByIdAsync(spec);

            if (profile is null)
                return Error.NotFound("User.NotFound", $"User with Id {userId} Is Not Found");

            return _mapper.Map<MyProfileDTO>(profile);
        }

        #endregion

    }
}
