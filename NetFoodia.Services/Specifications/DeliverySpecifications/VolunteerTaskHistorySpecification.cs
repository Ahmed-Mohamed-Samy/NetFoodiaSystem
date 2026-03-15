using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Domain.Entities.DeliveryModule;

namespace NetFoodia.Services.Specifications.DeliverySpecifications
{
    public class VolunteerTaskHistorySpecification : BaseSpecification<PickupTask>
    {
        public VolunteerTaskHistorySpecification(string volunteerId)
            : base(t => t.AssignedVolunteerId == volunteerId)
        {
            AddInclude(t => t.Donation);
            AddInclude(t => t.Charity);
            AddOrderByDesc(t => t.CreatedAt);
        }
    }
}
