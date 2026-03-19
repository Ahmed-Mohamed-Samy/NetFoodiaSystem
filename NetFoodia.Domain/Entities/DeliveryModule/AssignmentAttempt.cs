using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Domain.Entities.ProfileModule;
using NetFoodia.Domain.Entities.DonationModule;


namespace NetFoodia.Domain.Entities.DeliveryModule
{
    public class AssignmentAttempt : BaseEntity
    {
        public int PickupTaskId { get; set; }
        public PickupTask PickupTask { get; set; } = default!;

        public int DonationId { get; set; }
        public Donation Donation { get; set; } = default!;

        public string VolunteerId { get; set; } = default!;
        public VolunteerProfile Volunteer { get; set; } = default!;

        public DateTime OfferedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RespondedAt { get; set; }

        public AttemptResponse Response { get; set; } = AttemptResponse.Pending;
        public AttemptOutcome? Outcome { get; set; }

        public float DistanceKm { get; set; }
        public float EtaMinutes { get; set; }
        public int CandidateLoad { get; set; }
        public float? ScoreAtOffer { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
