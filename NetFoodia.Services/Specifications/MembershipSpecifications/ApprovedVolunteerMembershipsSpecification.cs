using NetFoodia.Domain.Entities.MembershipModule;

namespace NetFoodia.Services.Specifications.MembershipSpecifications
{
    public class ApprovedVolunteerMembershipsSpecification : BaseSpecification<VolunteerMembership>
    {
        public ApprovedVolunteerMembershipsSpecification()
            : base(m => m.Status == MembershipStatus.Approved)
        {
        }
    }
}
