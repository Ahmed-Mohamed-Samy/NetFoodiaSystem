using NetFoodia.Domain.Entities.MembershipModule;

namespace NetFoodia.Services.Specifications.DashboardSpecifications
{
    public class VolunteersByCharitySpecification : BaseSpecification<VolunteerMembership>
    {
        public VolunteersByCharitySpecification(int? charityId) : base(v => v.Status == MembershipStatus.Approved && (!charityId.HasValue || v.CharityId == charityId))
        {
        }
    }
}
