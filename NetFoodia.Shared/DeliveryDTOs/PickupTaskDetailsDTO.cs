using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Shared.DeliveryDTOs
{
    public class PickupTaskDetailsDTO
    {
        public int TaskId { get; set; }
        public int DonationId { get; set; }
        public int CharityId { get; set; }
        public string CharityName { get; set; } = default!;
        public string DonationTitle { get; set; } = default!;
        public string? AssignedVolunteerId { get; set; }
        public string? AssignedVolunteerName { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime? SlaDueAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
