using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Domain.Entities.DonationModule;

namespace NetFoodia.Services.Specifications.DonationSpecifications
{
    public class DonationByIdSpec : BaseSpecification<Donation>
    {
        public DonationByIdSpec(int donationId) : base(d => d.Id == donationId)
        {
            AddInclude(d => d.Charity);
            AddInclude(d => d.Donor);
            AddInclude(d => d.Donor.User);
        }
    }
}
