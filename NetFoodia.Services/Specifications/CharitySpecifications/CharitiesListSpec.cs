using NetFoodia.Domain.Entities.CharityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Services.Specifications.CharitySpecifications
{
    public class CharitiesListSpec : BaseSpecification<Charity>
    {
        public CharitiesListSpec(string? search, int pageIndex, int pageSize)
            : base(c => string.IsNullOrWhiteSpace(search) || c.OrganizationName.Contains(search))
        {
            AddOrderBy(c => c.OrganizationName);
            ApplyPagination(pageSize, pageIndex);
        }
    }
}
