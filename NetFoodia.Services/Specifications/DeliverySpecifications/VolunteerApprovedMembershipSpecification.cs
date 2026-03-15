using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Domain.Entities.MembershipModule;

namespace NetFoodia.Services.Specifications.DeliverySpecifications
{
    public class VolunteerApprovedMembershipSpecification : BaseSpecification<VolunteerMembership>
    {
        public VolunteerApprovedMembershipSpecification(string volunteerId, int charityId)
            : base(m => m.VolunteerId == volunteerId &&
                        m.CharityId == charityId &&
                        m.Status == MembershipStatus.Approved)
        {
        }
    }
}
