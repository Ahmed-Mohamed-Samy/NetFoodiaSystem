using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Domain.Entities.DonationModule;

namespace NetFoodia.Services.Specifications.DonationSpecifications
{
    public class DonorDonationByIdSpec : BaseSpecification<Donation>
    {
        public DonorDonationByIdSpec(string donorId, int donationId)
            : base(d => d.DonorId == donorId && d.Id == donationId)
        {
            AddInclude(d => d.Charity);
            AddInclude(d => d.Donor);
            AddInclude(d => d.Donor.User);
        }
    }
}
