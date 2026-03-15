using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Domain.Entities.DeliveryModule;

namespace NetFoodia.Services.Specifications.DeliverySpecifications
{
    public class HasOtherPendingOffersSpecification : BaseSpecification<AssignmentAttempt>
    {
        public HasOtherPendingOffersSpecification(int taskId, string currentVolunteerId)
            : base(a => a.PickupTaskId == taskId &&
                        a.VolunteerId != currentVolunteerId &&
                        a.Response == AttemptResponse.Pending)
        {
        }
    }
}
