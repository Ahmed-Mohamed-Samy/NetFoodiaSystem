using NetFoodia.Domain.Entities.CharityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Services.Specifications.CharitySpecifications
{
    public class CharityAdminProfileByUserSpec : BaseSpecification<CharityAdminProfile>
    {
        public CharityAdminProfileByUserSpec(string userId) : base(p => p.UserId == userId)
        {
            AddInclude(p => p.Charity);
        }
    }
}
