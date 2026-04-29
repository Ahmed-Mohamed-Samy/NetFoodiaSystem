using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.DonationDTOs;

namespace NetFoodia.Services_Abstraction
{
    /// <summary>
    /// Reporting service for donation analytics grouped by <c>UnitType</c> (Kilos vs Meals).
    /// Lives in the service-abstraction layer so it can be injected via DI without
    /// depending on the concrete implementation.
    /// </summary>
    public interface IDonationReportService
    {
        /// <summary>
        /// Builds a report that groups donations by <c>UnitType</c> (Kilos / Meals),
        /// then further by <c>FoodType</c>, with quantity totals and shelf-life statistics.
        /// All filtering is executed at the database level via a specification.
        /// </summary>
        /// <param name="charityId">Restrict the report to one charity (null = all charities).</param>
        /// <param name="from">Inclusive lower bound on <c>CreatedAt</c> (UTC).</param>
        /// <param name="to">Inclusive upper bound on <c>CreatedAt</c> (UTC).</param>
        /// <param name="unitType">
        /// When provided, only donations with this <see cref="UnitType"/> are included.
        /// Omit to include both Kilos and Meals.
        /// </param>
        /// <param name="foodType">
        /// When provided, only donations of this <see cref="FoodType"/> are included.
        /// Omit to include all food categories.
        /// </param>
        Task<Result<DonationUnitTypeReportDTO>> GetDonationsByUnitTypeAsync(
            int?      charityId = null,
            DateTime? from      = null,
            DateTime? to        = null,
            UnitType? unitType  = null,
            FoodType? foodType  = null);
    }
}
