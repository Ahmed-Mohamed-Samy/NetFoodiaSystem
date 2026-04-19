using NetFoodia.Domain.Entities.CharityModule;

namespace NetFoodia.Services.Specifications.CharitySpecifications
{
    public class ActiveAndVerfiedCharitiesListSpec : BaseSpecification<Charity>
    {
        public ActiveAndVerfiedCharitiesListSpec(string? search, int pageIndex, int pageSize) : base(c =>
        (string.IsNullOrWhiteSpace(search) || c.OrganizationName.Contains(search))
        && c.MembershipStatus == CharityMembershipStatus.Active
        && c.IsVerified)
        {
            AddOrderBy(c => c.OrganizationName);
            ApplyPagination(pageSize, pageIndex);
        }
    }
}
