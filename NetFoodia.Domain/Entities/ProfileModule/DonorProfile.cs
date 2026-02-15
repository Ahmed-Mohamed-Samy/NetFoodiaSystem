using NetFoodia.Domain.Entities.IdentityModule;
using NetFoodia.Domain.Entities.SharedValueObjects;

namespace NetFoodia.Domain.Entities.ProfileModule
{
    public class DonorProfile : BaseEntity
    {
        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;
        public bool IsBusiness { get; set; }
        public string? BusinessType { get; set; }
        public GeoLocation? Location { get; set; }
        public bool IsVerified { get; set; }
        public decimal ReliabilityScore { get; set; }
    }
}
