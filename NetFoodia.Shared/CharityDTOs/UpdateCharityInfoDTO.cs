using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Shared.CharityDTOs
{
    public class UpdateCharityInfoDTO
    {
        public string OrganizationName { get; set; } = default!;
        public string LicenseNumber { get; set; } = default!;
    }
}
