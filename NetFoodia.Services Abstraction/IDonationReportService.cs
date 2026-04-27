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
        /// Builds a report that groups all donations by <c>UnitType</c> (Kilos / Meals),
        /// then further by <c>FoodType</c>, with quantity totals and shelf-life statistics.
        /// </summary>
        /// <param name="charityId">
        /// When provided, restricts the report to donations for that charity only.
        /// </param>
        /// <param name="from">Optional inclusive lower bound on <c>CreatedAt</c> (UTC).</param>
        /// <param name="to">Optional inclusive upper bound on <c>CreatedAt</c> (UTC).</param>
        Task<Result<DonationUnitTypeReportDTO>> GetDonationsByUnitTypeAsync(
            int? charityId = null,
            DateTime? from = null,
            DateTime? to   = null);
    }
}
