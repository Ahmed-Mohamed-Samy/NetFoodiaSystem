using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Shared.DeliveryDTOs
{
    public class OpenTaskListItemDTO
    {
        public int TaskId { get; set; }
        public int DonationId { get; set; }
        public string DonationTitle { get; set; } = default!;
        public DateTime? SlaDueAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
