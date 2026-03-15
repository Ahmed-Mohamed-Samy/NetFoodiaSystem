using AutoMapper;
using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities.CharityModule;
using NetFoodia.Domain.Entities.DonationModule;
using NetFoodia.Domain.Entities.ProfileModule;
using NetFoodia.Services.Specifications.DonationSpecifications;
using NetFoodia.Services.Specifications.ProfileSpecifications;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.DonationDTOs;
using DonationStatus = NetFoodia.Domain.Entities.DonationModule.DonationStatus;

namespace NetFoodia.Services
{
    public class DonationService : IDonationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DonationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<DonationDetailsDTO>> CreateDonationAsync(string donorId, int charityId, CreateDonationDTO dto)
        {
            var donorRepo = _unitOfWork.GetRepository<DonorProfile>();
            var charityRepo = _unitOfWork.GetRepository<Charity>();
            var donationRepo = _unitOfWork.GetRepository<Donation>();

            var donor = await donorRepo.FirstOrDefaultAsync(new DonorProfileSpecification(donorId));
            if (donor is null)
                return Error.NotFound("DonorProfile.NotFound", $"Donor profile for user {donorId} not found");

            var charity = await charityRepo.GetByIdAsync(charityId);
            if (charity is null || !charity.IsVerified)
                return Error.NotFound("Charity.NotFound", $"Charity with Id {charityId} not found or not verified");

            var donation = _mapper.Map<Donation>(dto);
            donation.DonorId = donorId;
            donation.CharityId = charityId;
            donation.Status = DonationStatus.Pending;
            donation.UrgencyScore = CalculateUrgencyScore(donation.ExpirationTime);

            await donationRepo.AddAsync(donation);

            var result = await _unitOfWork.SaveChangesAsync() > 0;
            if (!result)
                return Error.Failure("Donation.CreateFailed", "Failed to create donation");

            var savedDonation = await donationRepo.GetByIdAsync(new DonationByIdSpec(donation.Id));
            var donationDto = _mapper.Map<DonationDetailsDTO>(savedDonation);

            return donationDto;
        }

        public async Task<Result> EditDonationAsync(string donorId, int donationId, EditDonationDTO dto)
        {
            var donationRepo = _unitOfWork.GetRepository<Donation>();

            var donation = await donationRepo.GetByIdAsync(new DonorDonationByIdSpec(donorId, donationId));
            if (donation is null)
                return Result.Fail(Error.NotFound("Donation.NotFound", "Donation not found"));

            if (donation.Status != DonationStatus.Pending)
                return Result.Fail(Error.Validation("Donation.InvalidState", "Only Pending donation can be edited"));

            _mapper.Map(dto, donation);
            donation.UrgencyScore = CalculateUrgencyScore(donation.ExpirationTime);

            donationRepo.Update(donation);
            await _unitOfWork.SaveChangesAsync();

            return Result.OK();
        }

        public async Task<Result> CancelDonationAsync(string donorId, int donationId)
        {
            var donationRepo = _unitOfWork.GetRepository<Donation>();

            var donation = await donationRepo.GetByIdAsync(new DonorDonationByIdSpec(donorId, donationId));
            if (donation is null)
                return Result.Fail(Error.NotFound("Donation.NotFound", "Donation not found"));

            if (donation.Status != DonationStatus.Pending)
                return Result.Fail(Error.Validation("Donation.InvalidState", "Only Pending donation can be cancelled"));

            donation.Status = DonationStatus.Cancelled;

            donationRepo.Update(donation);
            await _unitOfWork.SaveChangesAsync();

            return Result.OK();
        }

        public async Task<Result<IEnumerable<DonationListItemDTO>>> GetMyDonationsAsync(string donorId)
        {
            var donationRepo = _unitOfWork.GetRepository<Donation>();

            var donations = await donationRepo.GetAllAsync(new MyDonationsSpec(donorId));
            var donationsDto = _mapper.Map<IEnumerable<DonationListItemDTO>>(donations);

            return Result<IEnumerable<DonationListItemDTO>>.OK(donationsDto);
        }

        public async Task<Result<DonationDetailsDTO>> GetDonationDetailsAsync(string donorId, int donationId)
        {
            var donationRepo = _unitOfWork.GetRepository<Donation>();

            var donation = await donationRepo.GetByIdAsync(new DonorDonationByIdSpec(donorId, donationId));
            if (donation is null)
                return Error.NotFound("Donation.NotFound", "Donation not found");

            var donationDto = _mapper.Map<DonationDetailsDTO>(donation);
            return donationDto;
        }

        public async Task<Result<DonationStatusDTO>> TrackDonationStatusAsync(string donorId, int donationId)
        {
            var donationRepo = _unitOfWork.GetRepository<Donation>();

            var donation = await donationRepo.GetByIdAsync(new DonorDonationByIdSpec(donorId, donationId));
            if (donation is null)
                return Error.NotFound("Donation.NotFound", "Donation not found");

            var statusDto = _mapper.Map<DonationStatusDTO>(donation);
            return statusDto;
        }

        #region Helper Method
        private float CalculateUrgencyScore(DateTime expirationTime)
        {
            var hoursLeft = (float)(expirationTime - DateTime.UtcNow).TotalHours;

            if (hoursLeft <= 0) return 100f;
            if (hoursLeft <= 2) return 95f;
            if (hoursLeft <= 6) return 80f;
            if (hoursLeft <= 12) return 60f;
            if (hoursLeft <= 24) return 40f;

            return 20f;
        } 
        #endregion
    }
}