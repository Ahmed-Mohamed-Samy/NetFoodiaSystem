using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Shared.CharityDTOs
{
    public class CharityListItemDTO
    {
        public int CharityId { get; set; }
        public string OrganizationName { get; set; } = default!;
        public string? City { get; set; }
        public string? Governorate { get; set; }
        public bool IsVerified { get; set; }
    }
}
