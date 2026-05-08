using NetFoodia.Domain.Entities.CharityModule;

namespace NetFoodia.Services.Specifications.CharitySpecifications
{
    public class ActiveCharitiesSpecification : BaseSpecification<Charity>
    {
        public ActiveCharitiesSpecification()
            : base(c => c.IsVerified && c.MembershipStatus == CharityMembershipStatus.Active)
        {
        }
    }
}
