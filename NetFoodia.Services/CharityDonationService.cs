using AutoMapper;
using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities.CharityModule;
using NetFoodia.Domain.Entities.DeliveryModule;
using NetFoodia.Domain.Entities.DonationModule;
using NetFoodia.Services.Specifications.CharitySpecifications;
using NetFoodia.Services.Specifications.DeliverySpecifications;
using NetFoodia.Services.Specifications.DonationSpecifications;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.DonationDTOs;
using DonationStatus = NetFoodia.Domain.Entities.DonationModule.DonationStatus;

namespace NetFoodia.Services
{
    public class CharityDonationService : ICharityDonationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CharityDonationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<PendingDonationListItemDTO>>> ListPendingDonationsAsync(string charityAdminUserId)
        {
            var charityId = await GetCharityIdForAdmin(charityAdminUserId);
            if (charityId is null)
                return Error.NotFound("Charity.NotFound", "Charity not found for current admin");

            var repo = _unitOfWork.GetRepository<Donation>();
            var donations = await repo.GetAllAsync(new PendingDonationsForCharitySpec(charityId.Value));

            var donationsDto = _mapper.Map<IEnumerable<PendingDonationListItemDTO>>(donations);
            return Result<IEnumerable<PendingDonationListItemDTO>>.OK(donationsDto);
        }

        public async Task<Result<bool>> AcceptDonationAsync(string charityAdminUserId, int donationId)
        {
            var charityId = await GetCharityIdForAdmin(charityAdminUserId);
            if (charityId is null)
                return Error.NotFound("Charity.NotFound", "Charity not found for current admin");

            var repo = _unitOfWork.GetRepository<Donation>();
            var donation = await repo.GetByIdAsync(new DonationForCharityAdminSpec(charityId.Value, donationId));

            if (donation is null)
                return Error.NotFound("Donation.NotFound", "Donation not found");

            if (donation.Status != DonationStatus.Pending)
                return Error.Validation("Donation.InvalidState", "Only Pending donation can be accepted");

            donation.Status = DonationStatus.Accepted;
            donation.AcceptedAt = DateTime.UtcNow;

            repo.Update(donation);
            var result = await _unitOfWork.SaveChangesAsync() > 0;

            return result;
        }

        public async Task<Result<bool>> RejectDonationAsync(string charityAdminUserId, int donationId, string reason)
        {
            var charityId = await GetCharityIdForAdmin(charityAdminUserId);
            if (charityId is null)
                return Error.NotFound("Charity.NotFound", "Charity not found for current admin");

            var repo = _unitOfWork.GetRepository<Donation>();
            var donation = await repo.GetByIdAsync(new DonationForCharityAdminSpec(charityId.Value, donationId));

            if (donation is null)
                return Error.NotFound("Donation.NotFound", "Donation not found");

            if (donation.Status != DonationStatus.Pending)
                return Error.Validation("Donation.InvalidState", "Only Pending donation can be rejected");

            donation.Status = DonationStatus.Rejected;

            repo.Update(donation);
            var result = await _unitOfWork.SaveChangesAsync() > 0;

            return result;
        }

        public async Task<Result<bool>> MarkDonationExpiredAsync(string charityAdminUserId, int donationId)
        {
            var charityId = await GetCharityIdForAdmin(charityAdminUserId);
            if (charityId is null)
                return Error.NotFound("Charity.NotFound", "Charity not found for current admin");

            var repo = _unitOfWork.GetRepository<Donation>();
            var donation = await repo.GetByIdAsync(new DonationForCharityAdminSpec(charityId.Value, donationId));

            if (donation is null)
                return Error.NotFound("Donation.NotFound", "Donation not found");

            if (donation.Status == DonationStatus.Cancelled ||
                donation.Status == DonationStatus.Rejected ||
                donation.Status == DonationStatus.Expired)
            {
                return Error.Validation("Donation.InvalidState", "Donation can not be marked expired in its current state");
            }

            donation.Status = DonationStatus.Expired;

            repo.Update(donation);
            var result = await _unitOfWork.SaveChangesAsync() > 0;

            return result;
        }

        private async Task<int?> GetCharityIdForAdmin(string userId)
        {
            var repo = _unitOfWork.GetRepository<CharityAdminProfile>();
            var profile = await repo.FirstOrDefaultAsync(new CharityAdminProfileByUserSpec(userId));

            return profile?.CharityId;
        }


        public async Task<Result<IEnumerable<AcceptedUnassignedDonationDTO>>> ListAcceptedUnassignedDonationsAsync(string charityAdminUserId)
        {
            var charityId = await GetCharityIdForAdmin(charityAdminUserId);
            if (charityId is null)
                return Error.NotFound("Charity.NotFound", "Charity not found for current admin");

            var donationRepo = _unitOfWork.GetRepository<Donation>();
            var taskRepo = _unitOfWork.GetRepository<PickupTask>();

            var donations = await donationRepo.GetAllAsync(
                new AcceptedUnassignedDonationsForCharitySpec(charityId.Value));

            var filteredDonations = new List<Donation>();

            foreach (var donation in donations)
            {
                var hasPickupTask = await taskRepo.AnyAsync(
                    new TaskByDonationSpecification(donation.Id));

                if (!hasPickupTask)
                    filteredDonations.Add(donation);
            }

            var data = _mapper.Map<IEnumerable<AcceptedUnassignedDonationDTO>>(filteredDonations);

            return Result<IEnumerable<AcceptedUnassignedDonationDTO>>.OK(data);
        }

        public async Task<Result<bool>> ConfirmReceiptAsync(string charityAdminUserId, int donationId, ConfirmReceiptDTO dto)
        {
            var charityId = await GetCharityIdForAdmin(charityAdminUserId);
            if (charityId is null)
                return Error.NotFound("Charity.NotFound", "Charity not found for current admin");

            var repo = _unitOfWork.GetRepository<Donation>();
            var donation = await repo.GetByIdAsync(new DonationForCharityAdminSpec(charityId.Value, donationId));

            if (donation is null)
                return Error.NotFound("Donation.NotFound", "Donation not found");

            // Assuming either InTransit or ReadyForPickup depending on the flow
            if (donation.Status != DonationStatus.InTransit && donation.Status != DonationStatus.ReadyForPickup)
                return Error.Validation("Donation.InvalidState", "Only donations InTransit or ReadyForPickup can be confirmed");

            donation.Status = DonationStatus.Completed;
            if (!string.IsNullOrWhiteSpace(dto.Notes))
            {
                donation.Notes = string.IsNullOrWhiteSpace(donation.Notes) 
                    ? $"Receipt notes: {dto.Notes}" 
                    : $"{donation.Notes}\nReceipt notes: {dto.Notes}";
            }

            repo.Update(donation);
            var result = await _unitOfWork.SaveChangesAsync() > 0;

            return result;
        }
    }
}