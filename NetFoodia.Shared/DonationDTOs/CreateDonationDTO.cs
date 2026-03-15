using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Shared.DonationDTOs
{
    public class CreateDonationDTO
    {
        public string FoodType { get; set; } = default!;
        public int Quantity { get; set; }
        public DateTime PreparedTime { get; set; }
        public DateTime ExpirationTime { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? Notes { get; set; }
    }
}
