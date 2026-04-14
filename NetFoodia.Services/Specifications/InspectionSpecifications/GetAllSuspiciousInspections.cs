using NetFoodia.Domain.Entities.InspectionModule;

namespace NetFoodia.Services.Specifications.InspectionSpecifications
{
    public class GetAllSuspiciousInspections : BaseSpecification<FoodInspection>
    {
        public GetAllSuspiciousInspections() : base(i => i.SafetyStatus == SafetyStatus.Suspicious)
        {
        }
    }
}
