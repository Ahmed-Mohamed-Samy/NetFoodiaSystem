using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Shared.DeliveryDTOs
{
    public class MyTaskHistoryDTO
    {
        public int TaskId { get; set; }
        public int DonationId { get; set; }
        public string DonationTitle { get; set; } = default!;
        public string CharityName { get; set; } = default!;
        public TaskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
