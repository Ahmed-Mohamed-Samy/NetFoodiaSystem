using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Domain.Entities.DeliveryModule
{
    public enum AttemptResponse
    {
        Pending = 1,
        Accepted = 2,
        Rejected = 3,
        NoResponse = 4
    }
}
