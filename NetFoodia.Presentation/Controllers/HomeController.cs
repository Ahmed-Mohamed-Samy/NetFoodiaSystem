using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.HomeDTOs;
using System.Threading.Tasks;

namespace NetFoodia.Presentation.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class HomeController : ApiBaseController
    {
        private readonly IHomeStatisticsService _homeStatisticsService;

        public HomeController(IHomeStatisticsService homeStatisticsService)
        {
            _homeStatisticsService = homeStatisticsService;
        }

        [HttpGet("impact-statistics")]
        public async Task<ActionResult<HomeStatisticsDto>> GetImpactStatistics()
        {
            var result = await _homeStatisticsService.GetImpactStatisticsAsync();
            return HandleResult(result);
        }
    }
}
