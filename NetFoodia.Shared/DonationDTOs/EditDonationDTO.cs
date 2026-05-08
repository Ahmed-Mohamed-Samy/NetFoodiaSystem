// FoodType and UnitType are defined in this same namespace (NetFoodia.Shared.DonationDTOs)

namespace NetFoodia.Shared.DonationDTOs
{
    public class EditDonationDTO
    {
        public FoodType FoodType { get; set; }

        /// <summary>Optional unit override — resolved automatically when null or 0.</summary>
        public UnitType? UnitType { get; set; }

        public int Quantity { get; set; }
        public DateTime PreparedTime { get; set; }

        // ExpirationTime is re-derived from FoodType + PreparedTime on every edit.

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? Notes { get; set; }
    }
}
