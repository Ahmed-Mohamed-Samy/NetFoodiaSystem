// All types (FoodType, UnitType, DonationStatus) live in NetFoodia.Shared.DonationDTOs

namespace NetFoodia.Shared.DonationDTOs
{
    public class DonationDetailsDTO
    {
        public string PictureUrl { get; set; } = default!;
        public int DonationId { get; set; }
        public string DonorId { get; set; } = default!;
        public string DonorName { get; set; } = default!;
        public int CharityId { get; set; }
        public string CharityName { get; set; } = default!;

        /// <summary>Human-readable food category name (e.g. "CookedMeal").</summary>
        public string FoodType { get; set; } = default!;

        /// <summary>Unit of measurement for <see cref="Quantity"/> (Kilos or Meals).</summary>
        public string UnitType { get; set; } = default!;

        public int Quantity { get; set; }

        public DateTime PreparedTime { get; set; }
        public DateTime ExpirationTime { get; set; }

        /// <summary>Computed hours remaining until expiry (negative means already expired).</summary>
        public double ShelfLifeRemainingHours { get; set; }

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public string? Notes { get; set; }
        public float UrgencyScore { get; set; }
        public DonationStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? AcceptedAt { get; set; }
        public DateTime? AssignedAt { get; set; }
        public DateTime? PickedUpAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }
}

