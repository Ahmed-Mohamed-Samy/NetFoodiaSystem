using NetFoodia.Domain.Entities.IdentityModule;

namespace NetFoodia.Domain.Entities.CharityModule
{
    public class CharityAdminProfile : BaseEntity
    {
        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;

        public int CharityId { get; set; }
        public Charity Charity { get; set; } = default!;
    }
}
