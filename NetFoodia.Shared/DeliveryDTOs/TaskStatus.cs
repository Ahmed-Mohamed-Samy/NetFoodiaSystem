using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Shared.DeliveryDTOs
{
    public enum TaskStatus
    {
        Open = 1,
        Offered = 2,
        Assigned = 3,
        InProgress = 4,
        Completed = 5,
        Failed = 6
    }
}
