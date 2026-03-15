using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Domain.Entities.CharityModule;
using NetFoodia.Domain.Entities.DonationModule;
using NetFoodia.Domain.Entities.ProfileModule;

namespace NetFoodia.Domain.Entities.DeliveryModule
{
    public class PickupTask : BaseEntity
    {
        public int DonationId { get; set; }
        public Donation Donation { get; set; } = default!;

        public int CharityId { get; set; }
        public Charity Charity { get; set; } = default!;

        public string? AssignedVolunteerId { get; set; }
        public VolunteerProfile? AssignedVolunteer { get; set; }

        public DateTime? SlaDueAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public TaskStatus Status { get; set; } = TaskStatus.Open;
    }
}
