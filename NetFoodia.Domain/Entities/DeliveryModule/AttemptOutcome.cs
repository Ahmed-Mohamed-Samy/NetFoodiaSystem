using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Domain.Entities.DeliveryModule
{
    public enum AttemptOutcome
    {
        Completed = 1,
        Cancelled = 2,
        Late = 3,
        Failed = 4
    }
}