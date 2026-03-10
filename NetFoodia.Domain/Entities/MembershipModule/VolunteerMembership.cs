using NetFoodia.Domain.Entities.CharityModule;
using NetFoodia.Domain.Entities.ProfileModule;

namespace NetFoodia.Domain.Entities.MembershipModule
{
    public class VolunteerMembership : BaseEntity
    {
        public string VolunteerId { get; set; } = default!;
        public VolunteerProfile Volunteer { get; set; } = default!;
        public int CharityId { get; set; }
        public Charity Charity { get; set; } = default!;
        public MembershipStatus Status { get; set; }
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
        public string? RejectionReason { get; set; }
        public string? SuspendReason { get; set; }
    }
}
