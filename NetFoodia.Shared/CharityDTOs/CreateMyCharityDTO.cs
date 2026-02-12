using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Shared.CharityDTOs
{
    public class CreateMyCharityDTO
    {
        public string OrganizationName { get; set; } = default!;
        public string LicenseNumber { get; set; } = default!;

        // Location
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Governorate { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
