using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFoodia.Domain.Entities.DeliveryModule;
using TaskStatus = NetFoodia.Domain.Entities.DeliveryModule.TaskStatus;

namespace NetFoodia.Services.Specifications.DeliverySpecifications
{
    public class CharityOpenTasksSpecification : BaseSpecification<PickupTask>
    {
        public CharityOpenTasksSpecification(int charityId)
            : base(t => t.CharityId == charityId &&
                        (t.Status == TaskStatus.Open || t.Status == TaskStatus.Offered))
        {
            AddInclude(t => t.Donation);
            AddOrderByDesc(t => t.CreatedAt);
        }
    }
}
