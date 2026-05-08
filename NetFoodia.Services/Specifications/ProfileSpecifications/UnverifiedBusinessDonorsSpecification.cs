using NetFoodia.Domain.Entities.ProfileModule;

namespace NetFoodia.Services.Specifications.ProfileSpecifications
{
    public class UnverifiedBusinessDonorsSpecification : BaseSpecification<DonorProfile>
    {
        public UnverifiedBusinessDonorsSpecification()
            : base(d => d.IsBusiness && !d.IsVerified)
        {
            AddInclude(d => d.User); // Include User to map FullName/Email if needed
        }
    }
}
