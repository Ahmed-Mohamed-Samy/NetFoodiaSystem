using NetFoodia.Domain.Entities.ProfileModule;

namespace NetFoodia.Services.Specifications.ProfileSpecifications
{
    public class VolunteerProfileSpecification : BaseSpecification<VolunteerProfile>
    {
        public VolunteerProfileSpecification(string userId) : base(U => U.UserId == userId)
        {
            AddInclude(U => U.User);
        }
    }
}
