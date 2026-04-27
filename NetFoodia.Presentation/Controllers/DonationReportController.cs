using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.DonationDTOs;

namespace NetFoodia.Presentation.Controllers
{
    /// <summary>
    /// Provides donation analytics reports grouped by UnitType (Kilos / Meals).
    /// Accessible to Charity managers and SuperAdmin only.
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
        /// quantity totals and shelf-life statistics.
        /// </summary>
        /// <param name="charityId">Filter to a specific charity (omit for all charities — SuperAdmin only).</param>
        /// <param name="from">Inclusive start date for CreatedAt filter (UTC, optional).</param>
        /// <param name="to">Inclusive end date for CreatedAt filter (UTC, optional).</param>
        [Authorize(Roles = "CharityManager,SuperAdmin")]
        [HttpGet("ByUnitType")]
        public async Task<ActionResult<DonationUnitTypeReportDTO>> GetByUnitType(
            [FromQuery] int?      charityId = null,
            [FromQuery] DateTime? from      = null,
            [FromQuery] DateTime? to        = null)
        {
            var result = await _reportService.GetDonationsByUnitTypeAsync(charityId, from, to);
            return HandleResult(result);
        }
    }
}
