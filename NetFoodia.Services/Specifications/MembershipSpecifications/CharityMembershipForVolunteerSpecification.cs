using NetFoodia.Domain.Entities.MembershipModule;

namespace NetFoodia.Services.Specifications.MembershipSpecifications
{
    public class CharityMembershipForVolunteerSpecification : BaseSpecification<VolunteerMembership>
    {
        public CharityMembershipForVolunteerSpecification(int charityId) : base(x => x.CharityId == charityId && x.Status != MembershipStatus.Rejected && x.Status != MembershipStatus.Pending)
        {
            AddInclude(x => x.Volunteer);
            AddInclude(x => x.Volunteer.User);
        }
    }
}
