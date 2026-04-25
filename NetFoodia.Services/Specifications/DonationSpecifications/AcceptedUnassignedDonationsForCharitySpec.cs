using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Domain.Entities.DonationModule;
using DonationStatus = NetFoodia.Domain.Entities.DonationModule.DonationStatus;

namespace NetFoodia.Services.Specifications.DonationSpecifications
{
    public class AcceptedUnassignedDonationsForCharitySpec : BaseSpecification<Donation>
    {
        public AcceptedUnassignedDonationsForCharitySpec(int charityId)
            : base(d => d.CharityId == charityId &&
                        d.Status == DonationStatus.Accepted &&
                        d.AssignedAt == null)
        {
            AddInclude(d => d.Donor);
            AddInclude(d => d.Donor.User);
            AddOrderByDesc(d => d.UrgencyScore);
        }
    }
}
