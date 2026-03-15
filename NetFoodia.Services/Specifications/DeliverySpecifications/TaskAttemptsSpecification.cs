using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Domain.Entities.DeliveryModule;

namespace NetFoodia.Services.Specifications.DeliverySpecifications
{
    public class TaskAttemptsSpecification : BaseSpecification<AssignmentAttempt>
    {
        public TaskAttemptsSpecification(int taskId)
            : base(a => a.PickupTaskId == taskId && a.Response == AttemptResponse.Pending)
        {
        }
    }
}