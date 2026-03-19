using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Domain.Entities.DeliveryModule;
using AttemptResponse = NetFoodia.Domain.Entities.DeliveryModule.AttemptResponse;

namespace NetFoodia.Services.Specifications.DeliverySpecifications
{
    public class ExpiredPendingOffersSpecification : BaseSpecification<AssignmentAttempt>
    {
        public ExpiredPendingOffersSpecification()
            : base(a => a.Response == AttemptResponse.Pending &&
                        a.ExpiresAt <= DateTime.UtcNow)
        {
            AddInclude(a => a.PickupTask);
        }
    }
}