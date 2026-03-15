using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Domain.Entities.DeliveryModule;

namespace NetFoodia.Services.Specifications.DeliverySpecifications
{
    public class VolunteerAssignedTaskSpecification : BaseSpecification<PickupTask>
    {
        public VolunteerAssignedTaskSpecification(string volunteerId, int taskId)
            : base(t => t.AssignedVolunteerId == volunteerId && t.Id == taskId)
        {
            AddInclude(t => t.Donation);
            AddInclude(t => t.Charity);
        }
    }
}
