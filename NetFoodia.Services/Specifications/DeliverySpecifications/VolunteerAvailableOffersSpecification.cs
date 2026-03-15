using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Domain.Entities.DeliveryModule;

namespace NetFoodia.Services.Specifications.DeliverySpecifications
{
    public class VolunteerAvailableOffersSpecification : BaseSpecification<AssignmentAttempt>
    {
        public VolunteerAvailableOffersSpecification(string volunteerId)
            : base(a => a.VolunteerId == volunteerId &&
                        a.Response == AttemptResponse.Pending)
        {
            AddInclude(a => a.PickupTask);
            AddInclude(a => a.PickupTask.Donation);
            AddInclude(a => a.PickupTask.Charity);
            AddOrderByDesc(a => a.OfferedAt);
        }
    }
}
