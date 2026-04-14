using NetFoodia.Domain.Entities.InspectionModule;

namespace NetFoodia.Services.Specifications.InspectionSpecifications
{
    public class InspectionsCountSpec : BaseSpecification<FoodInspection>
    {

        public InspectionsCountSpec(SafetyStatus? status)
            : base(x => !status.HasValue || x.SafetyStatus == status)
        {
        }
    }
}
