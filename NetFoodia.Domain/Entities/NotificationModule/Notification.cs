using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Domain.Entities.IdentityModule;

namespace NetFoodia.Domain.Entities.NotificationModule
{
    public class Notification : BaseEntity
    {
        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;

        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;

        public NotificationType Type { get; set; }
        public bool IsRead { get; set; } = false;

        public int? RelatedTaskId { get; set; }
        public int? RelatedDonationId { get; set; }
    }
}
