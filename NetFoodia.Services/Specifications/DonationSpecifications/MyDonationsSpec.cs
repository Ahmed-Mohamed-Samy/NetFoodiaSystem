using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Domain.Entities.DonationModule;

namespace NetFoodia.Services.Specifications.DonationSpecifications
{
    public class MyDonationsSpec : BaseSpecification<Donation>
    {
        public MyDonationsSpec(string donorId) : base(d => d.DonorId == donorId)
        {
            AddInclude(d => d.Charity);
            AddOrderByDesc(d => d.CreatedAt);
        }
    }
}
