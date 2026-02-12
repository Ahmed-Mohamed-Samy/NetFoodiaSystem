using NetFoodia.Domain.Entities.CharityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Services.Specifications.CharitySpecifications
{
    public class CharitiesCountSpec : BaseSpecification<Charity>
    {
        public CharitiesCountSpec(string? search)
            : base(c => string.IsNullOrWhiteSpace(search) || c.OrganizationName.Contains(search))
        { }
    }
}
