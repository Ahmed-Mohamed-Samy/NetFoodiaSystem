using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Domain.Entities.DeliveryModule;

namespace NetFoodia.Services.Specifications.DeliverySpecifications
{
    public class ActiveOfferForVolunteerAndTaskSpecification : BaseSpecification<AssignmentAttempt>
    {
        public ActiveOfferForVolunteerAndTaskSpecification(string volunteerId, int taskId)
            : base(a => a.VolunteerId == volunteerId &&
                        a.PickupTaskId == taskId &&
                        a.Response == AttemptResponse.Pending)
        {
            AddInclude(a => a.PickupTask);
            AddInclude(a => a.PickupTask.Donation);
            AddInclude(a => a.PickupTask.Charity);
        }
    }
}
