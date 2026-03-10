using NetFoodia.Domain.Entities.ProfileModule;

namespace NetFoodia.Services.Specifications.MembershipSpecifications
{
    public class UserSpecification : BaseSpecification<VolunteerProfile>
    {
        public UserSpecification(string userId) : base(x => x.UserId == userId)
        {
            AddInclude(x => x.User);
        }
    }
}
