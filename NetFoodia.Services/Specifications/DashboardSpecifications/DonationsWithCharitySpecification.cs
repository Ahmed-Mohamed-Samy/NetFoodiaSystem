using NetFoodia.Domain.Entities.DonationModule;

namespace NetFoodia.Services.Specifications.DashboardSpecifications
{
    public class DonationsWithCharitySpecification : BaseSpecification<Donation>
    {
        public DonationsWithCharitySpecification(int? charityId)
        : base(d => !charityId.HasValue || d.CharityId == charityId)
        {
            AddInclude(d => d.Charity);
        }
    }
}
