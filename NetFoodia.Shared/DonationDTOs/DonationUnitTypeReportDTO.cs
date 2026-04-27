// FoodType and UnitType are defined in this same namespace (NetFoodia.Shared.DonationDTOs)

namespace NetFoodia.Shared.DonationDTOs
{
    /// <summary>
    /// Summary row for a single food-category group inside a <see cref="UnitTypeReportDTO"/>.
    /// </summary>
    public class FoodTypeGroupDTO
    {
        /// <summary>Human-readable food category name (e.g. "CookedMeal").</summary>
        public string FoodType { get; set; } = default!;

        /// <summary>Total number of donation records in this food-type group.</summary>
        public int DonationCount { get; set; }

        /// <summary>Total quantity summed across all donations in this group.</summary>
        public int TotalQuantity { get; set; }

        /// <summary>
        /// Average hours of shelf life remaining at the time the report is generated.
        /// Negative values indicate that these donations are already past expiry.
        /// </summary>
        public double AverageShelfLifeRemainingHours { get; set; }
    }

    /// <summary>
    /// All donations that share the same <see cref="UnitType"/>, grouped further by food category.
    /// </summary>
    public class UnitTypeReportDTO
    {
        /// <summary>The measurement unit for this group ("Kilos" or "Meals").</summary>
        public string UnitType { get; set; } = default!;

        /// <summary>Total number of donations in this unit-type bucket.</summary>
        public int TotalDonations { get; set; }

        /// <summary>Aggregate quantity across all donations in this bucket.</summary>
        public int TotalQuantity { get; set; }

        /// <summary>Further breakdown by food category.</summary>
        public List<FoodTypeGroupDTO> ByFoodType { get; set; } = [];
    }

    /// <summary>
    /// Top-level report returned by <c>IDonationReportService.GetDonationsByUnitTypeAsync</c>.
    /// Contains one <see cref="UnitTypeReportDTO"/> per distinct <see cref="UnitType"/> value.
    /// </summary>
    public class DonationUnitTypeReportDTO
    {
        /// <summary>Inclusive start of the reporting window (UTC).</summary>
        public DateTime? From { get; set; }

        /// <summary>Inclusive end of the reporting window (UTC).</summary>
        public DateTime? To { get; set; }

        /// <summary>
        /// Optional charity filter — <c>null</c> means the report covers all charities.
        /// </summary>
        public int? CharityId { get; set; }

        /// <summary>UTC timestamp when this report was generated.</summary>
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        /// <summary>Grand total number of donations included in this report.</summary>
        public int GrandTotalDonations { get; set; }

        /// <summary>Grand total quantity across all unit types.</summary>
        public int GrandTotalQuantity { get; set; }

        /// <summary>One entry per distinct <see cref="UnitType"/>.</summary>
        public List<UnitTypeReportDTO> Groups { get; set; } = [];
    }
}
