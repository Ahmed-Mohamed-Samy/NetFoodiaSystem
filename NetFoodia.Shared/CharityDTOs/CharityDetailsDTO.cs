using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NetFoodia.Shared.CharityDTOs
{
    public class CharityDetailsDTO
    {
        public int CharityId { get; set; }
        public string OrganizationName { get; set; } = default!;
        public string LicenseNumber { get; set; } = default!;
        public CharityMembershipStatus MembershipStatus { get; set; }
        public bool IsVerified { get; set; }

        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Governorate { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
