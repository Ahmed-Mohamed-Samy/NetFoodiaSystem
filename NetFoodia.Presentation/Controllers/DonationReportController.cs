using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.DonationDTOs;

namespace NetFoodia.Presentation.Controllers
{
    /// <summary>
    /// Provides donation analytics reports grouped by UnitType (Kilos / Meals).
    /// Accessible to CharityAdmin and Admin roles only.
    /// </summary>
    public class DonationReportController : ApiBaseController
    {
        private readonly IDonationReportService _reportService;

        public DonationReportController(IDonationReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Returns a report that groups donations by UnitType → FoodType with
        /// quantity totals and average shelf-life statistics.
        /// All filtering is applied at the database level.
        /// </summary>
        /// <param name="charityId">Filter to a specific charity (omit for all charities — Admin only).</param>
        /// <param name="from">Inclusive start date for CreatedAt filter (UTC, optional).</param>
        /// <param name="to">Inclusive end date for CreatedAt filter (UTC, optional).</param>
        /// <param name="unitType">
        /// Filter by unit of measurement: 1 = Kilos, 2 = Meals.
        /// Omit to include both.
        /// </param>
        /// <param name="foodType">
        /// Filter by food category:
        /// 1 = CookedMeal, 2 = Perishable, 3 = BakedGoods,
        /// 4 = NonPerishable, 5 = Beverage, 6 = Frozen.
        /// Omit to include all categories.
        /// </param>
        [Authorize(Roles = "CharityAdmin,Admin")]
        [HttpGet("ByUnitType")]
        public async Task<ActionResult<DonationUnitTypeReportDTO>> GetByUnitType(
            [FromQuery] int?      charityId = null,
            [FromQuery] DateTime? from      = null,
            [FromQuery] DateTime? to        = null,
            [FromQuery] UnitType? unitType  = null,
            [FromQuery] FoodType? foodType  = null)
        {
            var result = await _reportService.GetDonationsByUnitTypeAsync(
                charityId, from, to, unitType, foodType);

            return HandleResult(result);
        }
    }
}
