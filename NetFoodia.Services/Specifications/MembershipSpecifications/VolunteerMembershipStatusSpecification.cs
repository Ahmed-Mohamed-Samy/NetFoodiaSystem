using NetFoodia.Domain.Entities.MembershipModule;

namespace NetFoodia.Services.Specifications.MembershipSpecifications
{
    public class VolunteerMembershipStatusSpecification : BaseSpecification<VolunteerMembership>
    {
        public VolunteerMembershipStatusSpecification(string volunteerId) :
            base(x => x.VolunteerId == volunteerId &&
                        (x.Status == MembershipStatus.Pending
                         || x.Status == MembershipStatus.Approved
                         || x.Status == MembershipStatus.Suspended))
        {
            AddInclude(x => x.Charity);
        }
    }
}
