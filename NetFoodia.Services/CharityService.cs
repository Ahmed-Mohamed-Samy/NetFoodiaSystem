using AutoMapper;
using NetFoodia.Domain.Contracts;

using NetFoodia.Domain.Entities.CharityModule;
using NetFoodia.Services.Specifications.CharitySpecifications;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared;
using NetFoodia.Shared.CharityDTOs;
using NetFoodia.Shared.CommonResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Services
{
    public class CharityService : ICharityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CharityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<CharityDetailsDTO>> CreateMyCharityAsync(string userId, CreateMyCharityDTO dto)
        {
            var profileRepo = _unitOfWork.GetRepository<CharityAdminProfile>();
            var charityRepo = _unitOfWork.GetRepository<Charity>();

            var existingProfile = await profileRepo.GetByIdAsync(new CharityAdminProfileByUserSpec(userId));
            if (existingProfile is not null)
                return Error.Validation("Charity.Exists", "This Charity Admin already has a charity");

            
            var charity = _mapper.Map<Charity>(dto);

            
            charity.IsVerified = false;
            charity.MembershipStatus = NetFoodia.Domain.Entities.CharityModule.CharityMembershipStatus.Pending;

            await charityRepo.AddAsync(charity);
            await _unitOfWork.SaveChangesAsync();

            var profile = new CharityAdminProfile
            {
                UserId = userId,
                CharityId = charity.Id
            };

            await profileRepo.AddAsync(profile);
            await _unitOfWork.SaveChangesAsync();

           
            return _mapper.Map<CharityDetailsDTO>(charity);
        }

        public async Task<Result> UpdateCharityInfoAsync(string userId, UpdateCharityInfoDTO dto)
        {
            var profileRepo = _unitOfWork.GetRepository<CharityAdminProfile>();

            var profile = await profileRepo.GetByIdAsync(new CharityAdminProfileByUserSpec(userId));
            if (profile is null)
                return Result.Fail(Error.NotFound("Charity.NotFound", "No charity found for this admin"));

            var charity = profile.Charity;

          
            _mapper.Map(dto, charity);

            _unitOfWork.GetRepository<Charity>().Update(charity);
            await _unitOfWork.SaveChangesAsync();

            return Result.OK();
        }

        public async Task<Result> UpdateCharityLocationAsync(string userId, UpdateCharityLocationDTO dto)
        {
            var profileRepo = _unitOfWork.GetRepository<CharityAdminProfile>();

            var profile = await profileRepo.GetByIdAsync(new CharityAdminProfileByUserSpec(userId));
            if (profile is null)
                return Result.Fail(Error.NotFound("Charity.NotFound", "No charity found for this admin"));

            var charity = profile.Charity;

            _mapper.Map(dto, charity);

            _unitOfWork.GetRepository<Charity>().Update(charity);
            await _unitOfWork.SaveChangesAsync();

            return Result.OK();
        }

        public async Task<Result<CharityDetailsDTO>> GetCharityDetailsAsync(int charityId)
        {
            var charityRepo = _unitOfWork.GetRepository<Charity>();

            var charity = await charityRepo.GetByIdAsync(new CharityDetailsSpec(charityId));
            if (charity is null)
                return Error.NotFound("Charity.NotFound", "Charity not found");

            return _mapper.Map<CharityDetailsDTO>(charity);
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
