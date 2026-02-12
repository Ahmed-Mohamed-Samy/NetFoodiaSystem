using NetFoodia.Domain.Entities.CharityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Services.Specifications.CharitySpecifications
{
    public class CharityByIdSpec : BaseSpecification<Charity>
    {
        public CharityByIdSpec(int id) : base(c => c.Id == id)
        { }
    }
}
