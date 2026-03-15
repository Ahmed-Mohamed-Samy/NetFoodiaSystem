using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Domain.Entities.DeliveryModule;
using AttemptResponse = NetFoodia.Domain.Entities.DeliveryModule.AttemptResponse;

namespace NetFoodia.Services.Specifications.DeliverySpecifications
{
    public class AcceptedAttemptForVolunteerAndTaskSpecification : BaseSpecification<AssignmentAttempt>
    {
        public AcceptedAttemptForVolunteerAndTaskSpecification(string volunteerId, int taskId)
            : base(a => a.VolunteerId == volunteerId &&
                        a.PickupTaskId == taskId &&
                        a.Response == AttemptResponse.Accepted)
        {
            AddInclude(a => a.PickupTask);
            AddInclude(a => a.PickupTask.Donation);
            AddInclude(a => a.PickupTask.Charity);
        }
    }
}
