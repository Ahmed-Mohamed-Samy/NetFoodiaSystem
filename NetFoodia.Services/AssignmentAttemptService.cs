using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities.DeliveryModule;
using NetFoodia.Services.Specifications.DeliverySpecifications;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.CommonResult;
using AttemptResponse = NetFoodia.Domain.Entities.DeliveryModule.AttemptResponse;
using AttemptOutcome = NetFoodia.Domain.Entities.DeliveryModule.AttemptOutcome;
using TaskStatus = NetFoodia.Domain.Entities.DeliveryModule.TaskStatus;
using NotificationType = NetFoodia.Domain.Entities.NotificationModule.NotificationType;

namespace NetFoodia.Services
{
    public class AssignmentAttemptService : IAssignmentAttemptService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;

        public AssignmentAttemptService(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task<Result> ExpirePendingOffersAsync()
        {
            var attemptRepo = _unitOfWork.GetRepository<AssignmentAttempt>();
            var taskRepo = _unitOfWork.GetRepository<PickupTask>();

            var expiredAttempts = await attemptRepo.GetAllAsync(new ExpiredPendingOffersSpecification());

            foreach (var attempt in expiredAttempts)
            {
                attempt.Response = AttemptResponse.NoResponse;
                attempt.Outcome = AttemptOutcome.Failed;
                attempt.RespondedAt = DateTime.UtcNow;
                attemptRepo.Update(attempt);

                var task = attempt.PickupTask;
                var hasOtherPendingOffers = await attemptRepo.AnyAsync(
                    new HasOtherPendingOffersSpecification(task.Id, attempt.VolunteerId));

                task.Status = hasOtherPendingOffers ? TaskStatus.Offered : TaskStatus.Open;
                taskRepo.Update(task);

                await _notificationService.CreateNotificationAsync(
                    attempt.VolunteerId,
                    "Offer Expired",
                    "Your pickup offer expired because you did not respond in time.",
                    (int)Domain.Entities.NotificationModule.NotificationType.OfferReceived,
                    relatedTaskId: task.Id,
                    relatedDonationId: task.DonationId
                );
            }

            await _unitOfWork.SaveChangesAsync();
            return Result.OK();
        }
    }
}
