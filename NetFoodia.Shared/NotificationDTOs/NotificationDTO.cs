using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Shared.NotificationDTOs
{
    public class NotificationDTO
    {
        public int NotificationId { get; set; }
        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; }
        public int? RelatedTaskId { get; set; }
        public int? RelatedDonationId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}