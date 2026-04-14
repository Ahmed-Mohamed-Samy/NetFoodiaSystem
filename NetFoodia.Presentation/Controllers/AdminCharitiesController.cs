using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetFoodia.Services_Abstraction;

namespace NetFoodia.Presentation.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/Admin/Charities")]
    public class AdminCharitiesController : ApiBaseController
    {
        private readonly IAdminCharityService _adminCharityService;

        public AdminCharitiesController(IAdminCharityService adminCharityService)
        {
            _adminCharityService = adminCharityService;
        }

        [HttpPut("{id:int}/Verify")]
        public async Task<IActionResult> Verify(int id)
        {
            var result = await _adminCharityService.VerifyCharityAsync(id);
            return HandleResult(result);
        }

        [HttpPut("{id:int}/Deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await _adminCharityService.DeactivateCharityAsync(id);
            return HandleResult(result);
        }

        [HttpPut("{id:int}/Reactivate")]
        public async Task<IActionResult> Reactivate(int id)
        {
            var result = await _adminCharityService.ReactivateCharityAsync(id);
            return HandleResult(result);
        }
    }
}