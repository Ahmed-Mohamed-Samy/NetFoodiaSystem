using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Domain.Entities.DonationModule;

namespace NetFoodia.Services.Specifications.DonationSpecifications
{
    public class PendingDonationsForCharitySpec : BaseSpecification<Donation>
    {
        public PendingDonationsForCharitySpec(int charityId)
            : base(d => d.CharityId == charityId && d.Status == DonationStatus.Pending)
        {
            AddInclude(d => d.Donor);
            AddInclude(d => d.Donor.User);
            AddOrderByDesc(d => d.CreatedAt);
        }
    }
}
