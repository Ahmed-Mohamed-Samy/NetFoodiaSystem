using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Shared.NotificationDTOs
{
    public enum NotificationType
    {
        OfferReceived = 1,
        OfferExpired = 2,
        OfferCancelled = 3,
        TaskAssigned = 4,
        TaskReassigned = 5
    }
}