using AutoMapper;
using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities.CharityModule;
using NetFoodia.Domain.Entities.DeliveryModule;
using NetFoodia.Domain.Entities.DonationModule;
using NetFoodia.Domain.Entities.MembershipModule;
using NetFoodia.Services.Specifications.CharitySpecifications;
using NetFoodia.Services.Specifications.DeliverySpecifications;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.DeliveryDTOs;
using DonationStatus = NetFoodia.Domain.Entities.DonationModule.DonationStatus;
using TaskStatus = NetFoodia.Domain.Entities.DeliveryModule.TaskStatus;
using AttemptResponse = NetFoodia.Domain.Entities.DeliveryModule.AttemptResponse;
using AttemptOutcome = NetFoodia.Domain.Entities.DeliveryModule.AttemptOutcome;

namespace NetFoodia.Services
{
    public class CharityPickupTaskService : ICharityPickupTaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IAssignmentAttemptService _assignmentAttemptService;

        public CharityPickupTaskService(
             IUnitOfWork unitOfWork,
             IMapper mapper,
             INotificationService notificationService,
             IAssignmentAttemptService assignmentAttemptService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
            _assignmentAttemptService = assignmentAttemptService;
        }

        public async Task<Result<PickupTaskDetailsDTO>> CreatePickupTaskAsync(string charityAdminUserId, int donationId, CreatePickupTaskDTO dto)
        {
            var charityId = await GetCharityIdForAdmin(charityAdminUserId);
            if (charityId is null)
                return Error.NotFound("Charity.NotFound", "Charity not found for current admin");

            var donationRepo = _unitOfWork.GetRepository<Donation>();
            var taskRepo = _unitOfWork.GetRepository<PickupTask>();

            var donation = await donationRepo.GetByIdAsync(donationId);
            if (donation is null || donation.CharityId != charityId.Value)
                return Error.NotFound("Donation.NotFound", "Donation not found");

            if (donation.Status != DonationStatus.Accepted)
                return Error.Validation("Donation.InvalidState", "Pickup task can be created only for Accepted donation");

            var existingTask = await taskRepo.AnyAsync(new TaskByDonationSpecification(donationId));
            if (existingTask)
                return Error.Validation("PickupTask.AlreadyExists", "Pickup task already exists for this donation");

            var task = new PickupTask
            {
                DonationId = donationId,
                CharityId = charityId.Value,
                SlaDueAt = dto.SlaDueAt,
                Status = TaskStatus.Open
            };

            await taskRepo.AddAsync(task);

            var result = await _unitOfWork.SaveChangesAsync() > 0;
            if (!result)
                return Error.Failure("PickupTask.CreateFailed", "Failed to create pickup task");

            var savedTask = await taskRepo.GetByIdAsync(new PickupTaskByIdSpec(task.Id));
            var taskDto = _mapper.Map<PickupTaskDetailsDTO>(savedTask);

            return taskDto;
        }

        public async Task<Result<IEnumerable<OpenTaskListItemDTO>>> ListOpenTasksAsync(string charityAdminUserId)
        {
            var charityId = await GetCharityIdForAdmin(charityAdminUserId);
            if (charityId is null)
                return Error.NotFound("Charity.NotFound", "Charity not found for current admin");

            var repo = _unitOfWork.GetRepository<PickupTask>();
            var tasks = await repo.GetAllAsync(new CharityOpenTasksSpecification(charityId.Value));

            var tasksDto = _mapper.Map<IEnumerable<OpenTaskListItemDTO>>(tasks);
            return Result<IEnumerable<OpenTaskListItemDTO>>.OK(tasksDto);
        }

        public async Task<Result<bool>> OfferTaskToVolunteerAsync(string charityAdminUserId, int taskId, string volunteerUserId)
        {
            await _assignmentAttemptService.ExpirePendingOffersAsync();
            var charityId = await GetCharityIdForAdmin(charityAdminUserId);
            if (charityId is null)
                return Error.NotFound("Charity.NotFound", "Charity not found for current admin");

            var taskRepo = _unitOfWork.GetRepository<PickupTask>();
            var attemptRepo = _unitOfWork.GetRepository<AssignmentAttempt>();

            var task = await taskRepo.GetByIdAsync(new PickupTaskForCharitySpecification(charityId.Value, taskId));
            if (task is null)
                return Error.NotFound("PickupTask.NotFound", "Pickup task not found");

            if (task.Status != TaskStatus.Open && task.Status != TaskStatus.Offered)
                return Error.Validation("PickupTask.InvalidState", "Task must be Open or Offered");

            if (!await IsVolunteerApprovedForCharity(volunteerUserId, charityId.Value))
                return Error.Validation("Volunteer.InvalidMembership", "Volunteer is not approved for this charity");

            var existingOffer = await attemptRepo.AnyAsync(new ActiveOfferForVolunteerAndTaskSpecification(volunteerUserId, taskId));
            if (existingOffer)
                return Error.Validation("PickupTask.OfferExists", "Offer already exists for this volunteer");

            var offerTimeoutMinutes = 10;

            var attempt = new AssignmentAttempt
            {
                PickupTaskId = taskId,
                DonationId = task.DonationId,
                VolunteerId = volunteerUserId,
                OfferedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(offerTimeoutMinutes),
                Response = AttemptResponse.Pending,
                Outcome = null,
                DistanceKm = 0,
                EtaMinutes = 0,
                CandidateLoad = 0,
                ScoreAtOffer = null
            };

            await attemptRepo.AddAsync(attempt);

            task.Status = TaskStatus.Offered;
            taskRepo.Update(task);

            var result = await _unitOfWork.SaveChangesAsync() > 0;

            await _notificationService.CreateNotificationAsync(
                     volunteerUserId,
                     "New Pickup Offer",
                     $"You have a new pickup offer. Please respond before {attempt.ExpiresAt:yyyy-MM-dd HH:mm:ss}.",
                     (int)Domain.Entities.NotificationModule.NotificationType.OfferReceived,
                     relatedTaskId: taskId,
                     relatedDonationId: task.DonationId
                     );
            return result;
        }

        public async Task<Result<bool>> AssignTaskAsync(string charityAdminUserId, int taskId, string volunteerUserId)
        {
            var charityId = await GetCharityIdForAdmin(charityAdminUserId);
            if (charityId is null)
                return Error.NotFound("Charity.NotFound", "Charity not found for current admin");

            var taskRepo = _unitOfWork.GetRepository<PickupTask>();
            var donationRepo = _unitOfWork.GetRepository<Donation>();

            var task = await taskRepo.GetByIdAsync(new PickupTaskForCharitySpecification(charityId.Value, taskId));
            if (task is null)
                return Error.NotFound("PickupTask.NotFound", "Pickup task not found");

            if (task.Status != TaskStatus.Open && task.Status != TaskStatus.Offered)
                return Error.Validation("PickupTask.InvalidState", "Task must be Open or Offered");

            if (!await IsVolunteerApprovedForCharity(volunteerUserId, charityId.Value))
                return Error.Validation("Volunteer.InvalidMembership", "Volunteer is not approved for this charity");

            await CloseAllActiveAttempts(taskId);

            task.AssignedVolunteerId = volunteerUserId;
            task.Status = TaskStatus.Assigned;
            taskRepo.Update(task);

            var donation = await donationRepo.GetByIdAsync(task.DonationId);
            if (donation is not null)
            {
                donation.AssignedAt = DateTime.UtcNow;
                donationRepo.Update(donation);
            }

            var result = await _unitOfWork.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<Result<bool>> ReassignTaskAsync(string charityAdminUserId, int taskId)
        {
            var charityId = await GetCharityIdForAdmin(charityAdminUserId);
            if (charityId is null)
                return Error.NotFound("Charity.NotFound", "Charity not found for current admin");

            var taskRepo = _unitOfWork.GetRepository<PickupTask>();
            var donationRepo = _unitOfWork.GetRepository<Donation>();

            var task = await taskRepo.GetByIdAsync(new PickupTaskForCharitySpecification(charityId.Value, taskId));
            if (task is null)
                return Error.NotFound("PickupTask.NotFound", "Pickup task not found");

            if (task.Status != TaskStatus.Assigned && task.Status != TaskStatus.Offered)
                return Error.Validation("PickupTask.InvalidState", "Only Assigned or Offered task can be reassigned");

            await CloseAllActiveAttempts(taskId);

            task.AssignedVolunteerId = null;
            task.Status = TaskStatus.Open;
            taskRepo.Update(task);

            var donation = await donationRepo.GetByIdAsync(task.DonationId);
            if (donation is not null)
            {
                donation.AssignedAt = null;
                donationRepo.Update(donation);
            }

            var result = await _unitOfWork.SaveChangesAsync() > 0;
            return result;
        }

        private async Task<int?> GetCharityIdForAdmin(string userId)
        {
            var repo = _unitOfWork.GetRepository<CharityAdminProfile>();
            var profile = await repo.FirstOrDefaultAsync(new CharityAdminProfileByUserSpec(userId));
            return profile?.CharityId;
        }

        private async Task<bool> IsVolunteerApprovedForCharity(string volunteerUserId, int charityId)
        {
            var repo = _unitOfWork.GetRepository<VolunteerMembership>();
            return await repo.AnyAsync(new VolunteerApprovedMembershipSpecification(volunteerUserId, charityId));
        }

        private async Task CloseAllActiveAttempts(int taskId)
        {
            var repo = _unitOfWork.GetRepository<AssignmentAttempt>();
            var attempts = await repo.GetAllAsync(new TaskAttemptsSpecification(taskId));

            foreach (var attempt in attempts)
            {
                if (attempt.Response == AttemptResponse.Pending)
                {
                    attempt.Response = AttemptResponse.NoResponse;
                    attempt.RespondedAt = DateTime.UtcNow;
                    attempt.Outcome = AttemptOutcome.Failed;
                    repo.Update(attempt);
                }
            }
        }
    }
}