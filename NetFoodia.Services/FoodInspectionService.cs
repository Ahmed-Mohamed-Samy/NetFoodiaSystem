using AutoMapper;
using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities.DonationModule;
using NetFoodia.Domain.Entities.InspectionModule;
using NetFoodia.Services.SafetyRuleEngine;
using NetFoodia.Services.Specifications.InspectionSpecifications;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared;
using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.InspectionDTOs;

namespace NetFoodia.Services
{
    public class FoodInspectionService : IFoodInspectionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly RuleEngine _safetyRuleEngine;
        private readonly IFoodSafetyAIService _foodSafetyAIService;

        public FoodInspectionService(IUnitOfWork unitOfWork, IMapper mapper, RuleEngine safetyRuleEngine, IFoodSafetyAIService foodSafetyAIService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _safetyRuleEngine = safetyRuleEngine;
            _foodSafetyAIService = foodSafetyAIService;
        }

        public async Task<Result<bool>> CreateOrUpdateFromAI(int donationId)
        {
            var donationRepo = _unitOfWork.GetRepository<Donation>();
            var donation = await donationRepo.GetByIdAsync(donationId);
            if (donation is null)
                return Error.NotFound("Donation.NotFound", $"Donation with ID {donationId} not found");

            if (donation.Status != DonationStatus.Pending)
                return Error.Validation("Donation.Valdiation", "Donation must be pending");

            var webProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "NetFoodia.Web", "wwwroot");
            var fullPath = Path.Combine(webProjectPath, donation.ImagePath.TrimStart('/'));

            if (!File.Exists(fullPath))
                return Error.NotFound("File.NotFound", "Food image not found on server storage.");

            var imageBytes = await File.ReadAllBytesAsync(fullPath);


            var aiResult = await _foodSafetyAIService.PredictAsync(imageBytes);


            var calculatedStatus = _safetyRuleEngine.Evaluate(
                aiResult.IsSafe,
                aiResult.Confidence,
                donation.CreatedAt);


            var inspectionRepo = _unitOfWork.GetRepository<FoodInspection>();
            var spec = new GetByDonationIdSpec(donationId);
            var existingInspection = await inspectionRepo.FirstOrDefaultAsync(spec);

            if (existingInspection is null)
            {

                var newInspection = new FoodInspection
                {
                    DonationId = donationId,
                    SafetyStatus = calculatedStatus,
                    RiskScore = (float)aiResult.Confidence,
                    AiIsSafe = aiResult.IsSafe,
                    AiConfidence = (float)aiResult.Confidence,
                    Source = Domain.Entities.InspectionModule.InspectionSource.AI,
                };

                await inspectionRepo.AddAsync(newInspection);
            }
            else
            {

                existingInspection.SafetyStatus = calculatedStatus;
                existingInspection.RiskScore = (float)aiResult.Confidence;
                existingInspection.AiIsSafe = aiResult.IsSafe;
                existingInspection.AiConfidence = (float)aiResult.Confidence;
                existingInspection.Source = Domain.Entities.InspectionModule.InspectionSource.AI;
                existingInspection.UpdatedAt = DateTime.UtcNow;

                inspectionRepo.Update(existingInspection);
            }
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<Result<FoodInspectionDTO?>> GetByDonation(int donationId)
        {
            var inspectionRepo = _unitOfWork.GetRepository<FoodInspection>();
            var spec = new GetByDonationIdSpec(donationId);
            var inspection = await inspectionRepo.FirstOrDefaultAsync(spec);

            if (inspection is null)
                return Error.NotFound("Inspection.NotFound", $"Inspection with Donation Id {donationId} is not Found");

            return _mapper.Map<FoodInspectionDTO>(inspection);
        }

        public async Task<Result<IEnumerable<FoodInspectionDTO>>> GetSuspiciousInspections()
        {
            var inspectionRepo = _unitOfWork.GetRepository<FoodInspection>();


            var spec = new GetAllSuspiciousInspections();
            var suspicious = await inspectionRepo.GetAllAsync(spec);
            if (suspicious is null || !suspicious.Any())
                return Error.NotFound("Inspection.NotFound", "Suspicious Inspections Not Found");
            var suspiciousMapped = _mapper.Map<IEnumerable<FoodInspectionDTO>>(suspicious);


            return Result<IEnumerable<FoodInspectionDTO>>.OK(suspiciousMapped);
        }

        public async Task<Result<bool>> UpdateManual(int inspectionId, UpdateInspectionDTO dto, string adminId)
        {
            var inspectionRepo = _unitOfWork.GetRepository<FoodInspection>();
            var donationRepo = _unitOfWork.GetRepository<Donation>();
            var inspection = await inspectionRepo.GetByIdAsync(inspectionId);


            if (inspection is null)
                return Error.NotFound("Inspection.NotFound", "Inspection record not found");

            var donation = await donationRepo.GetByIdAsync(inspection.DonationId);
            if (donation is null)
                return Error.NotFound("Donation.NotFound", "Donation not found");

            if (donation.Status != DonationStatus.Pending)
                return Error.Validation("Donation.Validation", "Donation is no longer pending");

            inspection.SafetyStatus = (Domain.Entities.InspectionModule.SafetyStatus)dto.SafetyStatus;
            inspection.Notes = dto.Notes;
            inspection.Source = Domain.Entities.InspectionModule.InspectionSource.Manual;
            inspection.ReviewedByAdminId = adminId;
            inspection.UpdatedAt = DateTime.UtcNow;


            if (dto.RiskScore.HasValue)
                inspection.RiskScore = dto.RiskScore.Value;

            inspectionRepo.Update(inspection);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }


        public async Task<Result<PaginatedResult<FoodInspectionDTO>>> GetAllInspectionsPaginated(int pageIndex, int pageSize, string? status)
        {

            Domain.Entities.InspectionModule.SafetyStatus? statusEnum = null;
            if (Enum.TryParse<Domain.Entities.InspectionModule.SafetyStatus>(status, true, out var parsedStatus))
            {
                statusEnum = parsedStatus;
            }

            var spec = new InspectionsWithPaginationSpec(pageSize, pageIndex, statusEnum);
            var items = await _unitOfWork.GetRepository<FoodInspection>().GetAllAsync(spec);

            var countSpec = new InspectionsCountSpec(statusEnum);
            var totalItems = await _unitOfWork.GetRepository<FoodInspection>().CountAsync(countSpec);

            if (items is null || !items.Any())
                return Error.NotFound("Inspections.NotFound", "The Inspections Was Not Found");

            var data = _mapper.Map<IReadOnlyList<FoodInspectionDTO>>(items);

            return new PaginatedResult<FoodInspectionDTO>(pageIndex, pageSize, totalItems, data);
        }
    }
}
