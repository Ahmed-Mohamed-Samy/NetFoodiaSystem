using NetFoodia.Domain.Entities.MembershipModule;

namespace NetFoodia.Services.Specifications.DashboardSpecifications
{
    public class PendingRequestsSpecification : BaseSpecification<VolunteerMembership>
    {
        public PendingRequestsSpecification(int? charityId) : base(v => v.Status == MembershipStatus.Pending && (!charityId.HasValue || v.CharityId == charityId))
        {
        }
    }

}
