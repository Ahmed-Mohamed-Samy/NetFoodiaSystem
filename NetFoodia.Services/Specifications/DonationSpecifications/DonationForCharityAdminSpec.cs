using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Domain.Entities.DonationModule;

namespace NetFoodia.Services.Specifications.DonationSpecifications
{
    public class DonationForCharityAdminSpec : BaseSpecification<Donation>
    {
        public DonationForCharityAdminSpec(int charityId, int donationId)
            : base(d => d.CharityId == charityId && d.Id == donationId)
        {
            AddInclude(d => d.Donor);
            AddInclude(d => d.Donor.User);
            AddInclude(d => d.Charity);
        }
    }
}
