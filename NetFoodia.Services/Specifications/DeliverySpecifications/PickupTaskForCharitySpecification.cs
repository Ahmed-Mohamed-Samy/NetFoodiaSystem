using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Domain.Entities.DeliveryModule;

namespace NetFoodia.Services.Specifications.DeliverySpecifications
{
    public class PickupTaskForCharitySpecification : BaseSpecification<PickupTask>
    {
        public PickupTaskForCharitySpecification(int charityId, int taskId)
            : base(t => t.CharityId == charityId && t.Id == taskId)
        {
            AddInclude(t => t.Donation);
            AddInclude(t => t.Charity);
            AddInclude(t => t.AssignedVolunteer!);
            AddInclude(t => t.AssignedVolunteer!.User);
        }
    }
}
