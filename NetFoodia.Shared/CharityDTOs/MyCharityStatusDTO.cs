using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Shared.CharityDTOs
{
    public class MyCharityStatusDTO
    {
        public int CharityId { get; set; }
        public string OrganizationName { get; set; } = default!;
        public bool IsVerified { get; set; }
        public CharityMembershipStatus MembershipStatus { get; set; }
        public bool IsActive { get; set; }
    }
}
