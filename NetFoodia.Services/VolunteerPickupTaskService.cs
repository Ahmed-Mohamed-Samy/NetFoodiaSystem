using AutoMapper;
using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities.DeliveryModule;
using NetFoodia.Domain.Entities.DonationModule;
using NetFoodia.Services.Specifications.DeliverySpecifications;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.DeliveryDTOs;
using NetFoodia.Shared.DonationDTOs;
using AttemptResponse = NetFoodia.Domain.Entities.DeliveryModule.AttemptResponse;
using AttemptOutcome = NetFoodia.Domain.Entities.DeliveryModule.AttemptOutcome;
using TaskStatus = NetFoodia.Domain.Entities.DeliveryModule.TaskStatus;
using DonationStatus = NetFoodia.Domain.Entities.DonationModule.DonationStatus;

namespace NetFoodia.Services
{
    public class VolunteerPickupTaskService : IVolunteerPickupTaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAssignmentAttemptService _assignmentAttemptService;

        public VolunteerPickupTaskService(
                IUnitOfWork unitOfWork,
                IMapper mapper,
                IAssignmentAttemptService assignmentAttemptService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _assignmentAttemptService = assignmentAttemptService;
        }

        public async Task<Result<IEnumerable<VolunteerOfferDTO>>> ListAvailableOffersAsync(string volunteerUserId)
        {
            await _assignmentAttemptService.ExpirePendingOffersAsync();
            var repo = _unitOfWork.GetRepository<AssignmentAttempt>();
            var offers = await repo.GetAllAsync(new VolunteerAvailableOffersSpecification(volunteerUserId));

            var offersDto = _mapper.Map<IEnumerable<VolunteerOfferDTO>>(offers);
            return Result<IEnumerable<VolunteerOfferDTO>>.OK(offersDto);
        }

        public async Task<Result<bool>> AcceptTaskAsync(string volunteerUserId, int taskId)
        {
            await _assignmentAttemptService.ExpirePendingOffersAsync();
            var attemptRepo = _unitOfWork.GetRepository<AssignmentAttempt>();
            var taskRepo = _unitOfWork.GetRepository<PickupTask>();
            var donationRepo = _unitOfWork.GetRepository<Donation>();

            var attempt = await attemptRepo.FirstOrDefaultAsync(
                new ActiveOfferForVolunteerAndTaskSpecification(volunteerUserId, taskId));

            if (attempt is null)
                return Error.NotFound("AssignmentAttempt.NotFound", "Offer not found");

            var task = attempt.PickupTask;

            if (task.Status != TaskStatus.Open && task.Status != TaskStatus.Offered)
                return Error.Validation("PickupTask.InvalidState", "Task is not available for acceptance");

            attempt.Response = AttemptResponse.Accepted;
            attempt.Outcome = null;
            attempt.RespondedAt = DateTime.UtcNow;
            attemptRepo.Update(attempt);

            task.AssignedVolunteerId = volunteerUserId;
            task.Status = TaskStatus.Assigned;
            taskRepo.Update(task);

            var donation = await donationRepo.GetByIdAsync(task.DonationId);
            if (donation is not null)
            {
                donation.Status = DonationStatus.InspectionPending;
                donation.AssignedAt = DateTime.UtcNow;
                donationRepo.Update(donation);
            }

            await CloseOtherAttempts(taskId, volunteerUserId);

            var result = await _unitOfWork.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<Result<bool>> RejectTaskAsync(string volunteerUserId, int taskId)
        {
            await _assignmentAttemptService.ExpirePendingOffersAsync();
            var attemptRepo = _unitOfWork.GetRepository<AssignmentAttempt>();
            var taskRepo = _unitOfWork.GetRepository<PickupTask>();

            var attempt = await attemptRepo.FirstOrDefaultAsync(
                new ActiveOfferForVolunteerAndTaskSpecification(volunteerUserId, taskId));

            if (attempt is null)
                return Error.NotFound("AssignmentAttempt.NotFound", "Offer not found");

            attempt.Response = AttemptResponse.Rejected;
            attempt.Outcome = AttemptOutcome.Cancelled;
            attempt.RespondedAt = DateTime.UtcNow;
            attemptRepo.Update(attempt);

            var task = attempt.PickupTask;
            var hasOtherPendingOffers = await attemptRepo.AnyAsync(
                new HasOtherPendingOffersSpecification(taskId, volunteerUserId));

            task.Status = hasOtherPendingOffers ? TaskStatus.Offered : TaskStatus.Open;
            taskRepo.Update(task);

            var result = await _unitOfWork.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<Result<bool>> InspectDonationAsync(string volunteerUserId, int taskId, InspectDonationDTO dto)
        {
            var taskRepo = _unitOfWork.GetRepository<PickupTask>();
            var donationRepo = _unitOfWork.GetRepository<Donation>();

            var task = await taskRepo.GetByIdAsync(
                new VolunteerAssignedTaskSpecification(volunteerUserId, taskId));

            if (task is null)
                return Error.NotFound("PickupTask.NotFound", "Task not found");

            if (task.Status != TaskStatus.Assigned)
                return Error.Validation("PickupTask.InvalidState", "Only Assigned task can be inspected");

            var donation = await donationRepo.GetByIdAsync(task.DonationId);
            if (donation is null)
                return Error.NotFound("Donation.NotFound", "Donation not found");

            if (dto.IsApproved)
            {
                donation.Status = DonationStatus.ReadyForPickup;
            }
            else
            {
                donation.Status = DonationStatus.Rejected;
                if (!string.IsNullOrWhiteSpace(dto.Reason))
                {
                    donation.Notes = string.IsNullOrWhiteSpace(donation.Notes) 
                        ? $"Rejected at inspection: {dto.Reason}" 
                        : $"{donation.Notes}\nRejected at inspection: {dto.Reason}";
                }
                
                task.Status = TaskStatus.Failed;
                taskRepo.Update(task);
            }

            donationRepo.Update(donation);
            var result = await _unitOfWork.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<Result<bool>> StartPickupAsync(string volunteerUserId, int taskId)
        {
            var taskRepo = _unitOfWork.GetRepository<PickupTask>();
            var donationRepo = _unitOfWork.GetRepository<Donation>();

            var task = await taskRepo.GetByIdAsync(
                new VolunteerAssignedTaskSpecification(volunteerUserId, taskId));

            if (task is null)
                return Error.NotFound("PickupTask.NotFound", "Task not found");

            if (task.Status != TaskStatus.Assigned)
                return Error.Validation("PickupTask.InvalidState", "Only Assigned task can be started");

            var donation = await donationRepo.GetByIdAsync(task.DonationId);
            if (donation != null && donation.Status != DonationStatus.ReadyForPickup)
                return Error.Validation("Donation.InvalidState", "Donation must be inspected and approved before pickup");

            task.Status = TaskStatus.InProgress;
            task.StartedAt = DateTime.UtcNow;
            taskRepo.Update(task);

            if (donation is not null)
            {
                donation.Status = DonationStatus.InTransit;
                donation.PickedUpAt = DateTime.UtcNow;
                donationRepo.Update(donation);
            }

            var result = await _unitOfWork.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<Result<bool>> CompleteDeliveryAsync(string volunteerUserId, int taskId)
        {
            var taskRepo = _unitOfWork.GetRepository<PickupTask>();
            var donationRepo = _unitOfWork.GetRepository<Donation>();
            var attemptRepo = _unitOfWork.GetRepository<AssignmentAttempt>();

            var task = await taskRepo.GetByIdAsync(
                new VolunteerAssignedTaskSpecification(volunteerUserId, taskId));

            if (task is null)
                return Error.NotFound("PickupTask.NotFound", "Task not found");

            if (task.Status != TaskStatus.InProgress)
                return Error.Validation("PickupTask.InvalidState", "Only InProgress task can be completed");

            task.Status = TaskStatus.Completed;
            task.CompletedAt = DateTime.UtcNow;
            taskRepo.Update(task);

            var donation = await donationRepo.GetByIdAsync(task.DonationId);
            if (donation is not null)
            {
                donation.DeliveredAt = DateTime.UtcNow;
                donationRepo.Update(donation);
            }

            var attempt = await attemptRepo.FirstOrDefaultAsync(
                new AcceptedAttemptForVolunteerAndTaskSpecification(volunteerUserId, taskId));

            if (attempt is not null)
            {
                attempt.Outcome = AttemptOutcome.Completed;
                attemptRepo.Update(attempt);
            }

            var result = await _unitOfWork.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<Result<IEnumerable<MyTaskHistoryDTO>>> GetMyTasksHistoryAsync(string volunteerUserId)
        {
            var repo = _unitOfWork.GetRepository<PickupTask>();
            var tasks = await repo.GetAllAsync(new VolunteerTaskHistorySpecification(volunteerUserId));

            var tasksDto = _mapper.Map<IEnumerable<MyTaskHistoryDTO>>(tasks);
            return Result<IEnumerable<MyTaskHistoryDTO>>.OK(tasksDto);
        }

        private async Task CloseOtherAttempts(int taskId, string acceptedVolunteerId)
        {
            var repo = _unitOfWork.GetRepository<AssignmentAttempt>();
            var attempts = await repo.GetAllAsync(new TaskAttemptsSpecification(taskId));

            foreach (var attempt in attempts.Where(a => a.VolunteerId != acceptedVolunteerId))
            {
                if (attempt.Response == AttemptResponse.Pending)
                {
                    attempt.Response = AttemptResponse.NoResponse;
                    attempt.RespondedAt = DateTime.UtcNow;
                    attempt.Outcome = AttemptOutcome.Failed;
                }

                repo.Update(attempt);
            }
        }

        public async Task<Result<bool>> CancelAcceptedTaskAsync(string volunteerUserId, int taskId)
        {
            var attemptRepo = _unitOfWork.GetRepository<AssignmentAttempt>();
            var taskRepo = _unitOfWork.GetRepository<PickupTask>();
            var donationRepo = _unitOfWork.GetRepository<Donation>();

            var attempt = await attemptRepo.FirstOrDefaultAsync(
                new AcceptedAttemptForVolunteerAndTaskSpecification(volunteerUserId, taskId));

            if (attempt is null)
                return Error.NotFound("AssignmentAttempt.NotFound", "Accepted task not found");

            var task = attempt.PickupTask;

            if (task.AssignedVolunteerId != volunteerUserId)
                return Error.Validation("PickupTask.NotAssignedToVolunteer", "This task is not assigned to the current volunteer");

            if (task.Status != TaskStatus.Assigned)
                return Error.Validation("PickupTask.InvalidState", "Only Assigned task can be cancelled by volunteer");

            attempt.Outcome = AttemptOutcome.Cancelled;
            attempt.RespondedAt = DateTime.UtcNow;
            attemptRepo.Update(attempt);

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
    }
}