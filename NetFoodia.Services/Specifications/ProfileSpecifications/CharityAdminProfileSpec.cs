using NetFoodia.Domain.Entities.CharityModule;

namespace NetFoodia.Services.Specifications.ProfileSpecifications
{
    public class CharityAdminProfileSpec : BaseSpecification<CharityAdminProfile>
    {
        public CharityAdminProfileSpec(string userId) : base(U => U.UserId == userId)
        {
            AddInclude(U => U.User);
        }
    }
}
