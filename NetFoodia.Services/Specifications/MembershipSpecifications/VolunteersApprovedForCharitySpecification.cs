using NetFoodia.Domain.Entities.MembershipModule;

namespace NetFoodia.Services.Specifications.MembershipSpecifications
{
    public class VolunteersApprovedForCharitySpecification : BaseSpecification<VolunteerMembership>
    {
        public VolunteersApprovedForCharitySpecification(int charityId)
            : base(v => v.CharityId == charityId &&
                        v.Status == MembershipStatus.Approved)
        {
            AddInclude(v => v.Volunteer);
            AddInclude(v => v.Volunteer.User);
        }
    }
}