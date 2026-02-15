using NetFoodia.Domain.Entities.IdentityModule;
using NetFoodia.Domain.Entities.SharedValueObjects;

namespace NetFoodia.Domain.Entities.ProfileModule
{
    public class VolunteerProfile : BaseEntity
    {
        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;

        public VolunteerStatus Status { get; set; }
        public GeoLocation? Location { get; set; }
        public string? VehicleType { get; set; }
        public DateTime LastActiveAt { get; set; } = DateTime.UtcNow;
    }
}
