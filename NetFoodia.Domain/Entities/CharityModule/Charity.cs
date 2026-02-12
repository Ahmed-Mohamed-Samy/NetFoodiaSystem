using NetFoodia.Domain.Entities.SharedValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Domain.Entities.CharityModule
{
    public class Charity : BaseEntity
    {
        public string OrganizationName { get; set; } = default!;
        public string LicenseNumber { get; set; } = default!;
        public CharityMembershipStatus MembershipStatus { get; set; } = CharityMembershipStatus.Pending;
        public bool IsVerified { get; set; } = false;

        // Location
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Governorate { get; set; }
        public GeoLocation Location { get; set; } = default!;

        // Navigation
        public CharityAdminProfile AdminProfile { get; set; } = default!;
    }
}
