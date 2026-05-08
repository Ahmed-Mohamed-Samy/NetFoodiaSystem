using NetFoodia.Domain.Entities.MembershipModule;
using NetFoodia.Domain.Entities.ProfileModule;

namespace NetFoodia.Services.Specifications.MembershipSpecifications
{
    public class VolunteersApprovedForCharitySpecification : BaseSpecification<VolunteerMembership>
    {
        public VolunteersApprovedForCharitySpecification(int charityId)
            : base(v => v.CharityId == charityId &&
                        v.Status == MembershipStatus.Approved &&
                        v.Volunteer.Status == VolunteerStatus.Available)
        {
            AddInclude(v => v.Volunteer);
            AddInclude(v => v.Volunteer.User);
        }
    }
}