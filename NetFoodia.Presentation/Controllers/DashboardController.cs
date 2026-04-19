using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.DashboardDTOs;

namespace NetFoodia.Presentation.Controllers
{
    public class DashboardController : ApiBaseController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [Authorize(Roles = "Admin,CharityAdmin")]
        [HttpGet("stats")]
        public async Task<ActionResult<DashboardStatsDTO>> GetStats([FromQuery] int? charityId)
        {
            var userRole = GetRoleFromToken();


            if (userRole == "Admin")
            {
                var stats = await _dashboardService.GetStatsAsync(charityId);
                return HandleResult(stats);
            }


            if (userRole == "CharityAdmin")
            {
                var stats = await _dashboardService.GetStatsAsync(charityId);
                return HandleResult(stats);
            }

            return BadRequest("User role not authorized for dashboard.");
        }
    }
}
