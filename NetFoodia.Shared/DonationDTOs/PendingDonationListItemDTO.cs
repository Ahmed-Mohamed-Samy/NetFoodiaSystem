using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Shared.DonationDTOs
{
    public class PendingDonationListItemDTO
    {
        public int DonationId { get; set; }
        public string DonorName { get; set; } = default!;
        public string FoodType { get; set; } = default!;
        public int Quantity { get; set; }
        public DateTime ExpirationTime { get; set; }
        public float UrgencyScore { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
