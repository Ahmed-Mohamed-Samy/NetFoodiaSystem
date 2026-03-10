using NetFoodia.Domain.Entities.MembershipModule;

namespace NetFoodia.Services.Specifications.MembershipSpecifications
{
    public class PendingVolunteersSpecification : BaseSpecification<VolunteerMembership>
    {
        public PendingVolunteersSpecification(int charityId) : base(x => x.CharityId == charityId && x.Status == MembershipStatus.Pending)
        {
            AddInclude(x => x.Volunteer);
            AddInclude(x => x.Volunteer.User);
        }
    }
}
