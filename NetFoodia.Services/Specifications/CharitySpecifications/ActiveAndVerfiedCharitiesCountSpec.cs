using NetFoodia.Domain.Entities.CharityModule;

namespace NetFoodia.Services.Specifications.CharitySpecifications
{
    public class ActiveAndVerfiedCharitiesCountSpec : BaseSpecification<Charity>
    {
        public ActiveAndVerfiedCharitiesCountSpec(string? search)
            : base(c =>
                (string.IsNullOrWhiteSpace(search) || c.OrganizationName.Contains(search))
                && c.MembershipStatus == CharityMembershipStatus.Active
                && c.IsVerified)
        {
        }
    }
}
