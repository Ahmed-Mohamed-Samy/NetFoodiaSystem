using NetFoodia.Domain.Entities.DonationModule;

namespace NetFoodia.Services.Specifications.DonationSpecifications
{
    public class CompletedDonationsSpecification : BaseSpecification<Donation>
    {
        public CompletedDonationsSpecification()
            : base(d => d.Status == DonationStatus.Completed)
        {
        }
    }
}
