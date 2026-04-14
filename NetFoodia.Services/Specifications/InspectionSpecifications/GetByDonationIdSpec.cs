using NetFoodia.Domain.Entities.InspectionModule;

namespace NetFoodia.Services.Specifications.InspectionSpecifications
{
    public class GetByDonationIdSpec : BaseSpecification<FoodInspection>
    {
        public GetByDonationIdSpec(int donationId) : base(x => x.DonationId == donationId)
        {
        }
    }
}
