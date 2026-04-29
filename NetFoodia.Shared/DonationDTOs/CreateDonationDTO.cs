using Microsoft.AspNetCore.Http;
using NetFoodia.Shared.DonationDTOs;

namespace NetFoodia.Shared.DonationDTOs
{
    public class CreateDonationDTO
    {
        /// <summary>Category of the donated food — drives automatic expiry calculation.</summary>
        public FoodType FoodType { get; set; }

        /// <summary>
        /// Optional unit override. When null or 0, the system resolves the default
        /// based on <see cref="FoodType"/> (CookedMeal → Meals, all others → Kilos).
        /// </summary>
        public UnitType? UnitType { get; set; }

        public int Quantity { get; set; }
        public DateTime PreparedTime { get; set; }

        // ExpirationTime is intentionally absent — it is calculated server-side
        // via FoodExpiryPolicy to prevent invalid client-supplied values.

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? Notes { get; set; }
        public IFormFile Image { get; set; } = default!;
    }
}
