using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Shared.DeliveryDTOs
{
    public class VolunteerOfferDTO
    {
        public int TaskId { get; set; }
        public int DonationId { get; set; }
        public string DonationTitle { get; set; } = default!;
        public string CharityName { get; set; } = default!;
        public DateTime OfferedAt { get; set; }
        public DateTime? SlaDueAt { get; set; }
        public AttemptResponse Response { get; set; }
    }
}
