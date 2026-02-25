using NetFoodia.Domain.Entities.ProfileModule;

namespace NetFoodia.Services.Specifications.ProfileSpecifications
{
    public class DonorProfileSpecification : BaseSpecification<DonorProfile>
    {
        public DonorProfileSpecification(string userId) : base(U => U.UserId == userId)
        {
            AddInclude(U => U.User);
        }
    }
}
