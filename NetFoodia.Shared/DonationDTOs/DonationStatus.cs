using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Shared.DonationDTOs
{
    public enum DonationStatus
    {
        Pending = 1,
        Accepted = 2,
        Rejected = 3,
        Cancelled = 4,
        Expired = 5
    }
}
