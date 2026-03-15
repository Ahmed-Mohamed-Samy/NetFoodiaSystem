using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Shared.DonationDTOs
{
    public class DonationStatusDTO
    {
        public int DonationId { get; set; }
        public DonationStatus Status { get; set; }
        public DateTime? AcceptedAt { get; set; }
        public DateTime? AssignedAt { get; set; }
        public DateTime? PickedUpAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
