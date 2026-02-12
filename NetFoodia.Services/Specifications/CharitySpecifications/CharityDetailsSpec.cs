using NetFoodia.Domain.Entities.CharityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Services.Specifications.CharitySpecifications
{
    public class CharityDetailsSpec : BaseSpecification<Charity>
    {
        public CharityDetailsSpec(int charityId) : base(c => c.Id == charityId)
        { }
    }
}
