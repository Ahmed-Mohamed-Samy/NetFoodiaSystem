using NetFoodia.Domain.Entities.InspectionModule;

namespace NetFoodia.Services.Specifications.InspectionSpecifications
{
    public class InspectionsWithPaginationSpec : BaseSpecification<FoodInspection>
    {

        public InspectionsWithPaginationSpec(int pageSize, int pageIndex, SafetyStatus? status)
            : base(x => !status.HasValue || x.SafetyStatus == status)
        {

            AddInclude(x => x.Donation);


            AddOrderByDesc(x => x.CreatedAt);


            ApplyPagination(pageSize, pageIndex);
        }
    }
}
