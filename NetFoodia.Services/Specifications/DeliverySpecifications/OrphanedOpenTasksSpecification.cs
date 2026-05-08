using System;
using NetFoodia.Domain.Entities.DeliveryModule;
using TaskStatus = NetFoodia.Domain.Entities.DeliveryModule.TaskStatus;

namespace NetFoodia.Services.Specifications.DeliverySpecifications
{
    /// <summary>
    /// Finds all Open PickupTasks that have been sitting unmatched for longer than
    /// the specified grace period — these are "orphaned" tasks that need escalation.
    /// Used by <c>TaskEscalationWorker</c>.
    /// </summary>
    public class OrphanedOpenTasksSpecification : BaseSpecification<PickupTask>
    {
        public OrphanedOpenTasksSpecification()
            : base(t => t.Status == TaskStatus.Open && 
                        t.SlaDueAt.HasValue && 
                        DateTime.UtcNow > t.SlaDueAt &&
                        t.Donation.ExpirationTime > DateTime.UtcNow)
        {
            AddInclude(t => t.Donation);
        }
    }
}
