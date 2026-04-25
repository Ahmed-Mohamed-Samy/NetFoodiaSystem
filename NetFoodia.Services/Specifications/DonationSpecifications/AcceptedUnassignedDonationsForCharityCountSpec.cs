using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Domain.Entities.DonationModule;
using DonationStatus = NetFoodia.Domain.Entities.DonationModule.DonationStatus;

namespace NetFoodia.Services.Specifications.DonationSpecifications
{
    public class AcceptedUnassignedDonationsForCharityCountSpec : BaseSpecification<Donation>
    {
        public AcceptedUnassignedDonationsForCharityCountSpec(int charityId)
            : base(d => d.CharityId == charityId &&
                        d.Status == DonationStatus.Accepted &&
                        d.AssignedAt == null)
        {
        }
    }
}
