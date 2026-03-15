using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Shared.DonationDTOs
{
    public class DonationListItemDTO
    {
        public int DonationId { get; set; }
        public string DonorId { get; set; } = default!;
        public string DonorName { get; set; } = default!;
        public int CharityId { get; set; }
        public string CharityName { get; set; } = default!;

        public string FoodType { get; set; } = default!;
        public int Quantity { get; set; }

        public DateTime PreparedTime { get; set; }
        public DateTime ExpirationTime { get; set; }

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public string? Notes { get; set; }
        public float UrgencyScore { get; set; }
        public DonationStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? AcceptedAt { get; set; }
        public DateTime? AssignedAt { get; set; }
        public DateTime? PickedUpAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }
}
