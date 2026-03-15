using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Domain.Entities.DeliveryModule;

namespace NetFoodia.Services.Specifications.DeliverySpecifications
{
    public class TaskByDonationSpecification : BaseSpecification<PickupTask>
    {
        public TaskByDonationSpecification(int donationId)
            : base(t => t.DonationId == donationId)
        {
        }
    }
}
