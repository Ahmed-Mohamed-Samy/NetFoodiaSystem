using NetFoodia.Domain.Entities.DeliveryModule;

namespace NetFoodia.Services.Specifications.DeliverySpecifications
{
    /// <summary>
    /// Checks if a volunteer has EVER received an assignment attempt for a specific task,
    /// regardless of whether they accepted, rejected, or let it expire.
    /// Used by the TaskEscalationWorker to avoid spamming volunteers who already evaluated the task.
    /// </summary>
    public class AnyAttemptForVolunteerAndTaskSpecification : BaseSpecification<AssignmentAttempt>
    {
        public AnyAttemptForVolunteerAndTaskSpecification(string volunteerId, int taskId)
            : base(a => a.VolunteerId == volunteerId && a.PickupTaskId == taskId)
        {
        }
    }
}
