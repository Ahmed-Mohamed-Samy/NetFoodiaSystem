using NetFoodia.Domain.Entities.DeliveryModule;

namespace NetFoodia.Services.Specifications.DeliverySpecifications
{
    public class TaskByDonationSpecification : BaseSpecification<PickupTask>
    {
        public TaskByDonationSpecification(int donationId)
            : base(t => t.DonationId == donationId && t.Status != Domain.Entities.DeliveryModule.TaskStatus.Cancelled)
        {
        }
    }
}
