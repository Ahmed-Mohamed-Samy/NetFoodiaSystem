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

namespace NetFoodia.Services
{
    public class AdminDonorService : IAdminDonorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdminDonorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
