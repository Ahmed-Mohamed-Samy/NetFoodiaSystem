using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.ProfileDTOs;

namespace NetFoodia.Presentation.Controllers
{
    [Authorize]
    public class ProfileController : ApiBaseController
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }


        [HttpGet]
        public async Task<ActionResult<MyProfileDTO>> GetMyProfile()
        {
            var userId = GetUserIdFromToken();
            var role = GetRoleFromToken();

            var result = await _profileService.GetMyProfileAsync(userId, role);
            return HandleResult(result);
        }

        [Authorize(Roles = "Donor")]
        [HttpPost("donor")]
        public async Task<IActionResult> UpsertDonor([FromBody] UpsertDonorProfileDTO dto)
        {
            var userId = GetUserIdFromToken();

            var result = await _profileService.UpsertDonorAsync(userId, dto);

            return HandleResult(result);
        }


        [Authorize(Roles = "Volunteer")]
        [HttpPost("volunteer")]
        public async Task<IActionResult> UpsertVolunteer([FromBody] UpsertVolunteerProfileDTO dto)
        {
            var userId = GetUserIdFromToken();

            var result = await _profileService.UpsertVolunteerAsync(userId, dto);

            return HandleResult(result);
        }

        [Authorize(Roles = "Volunteer")]
        [HttpPatch("volunteer/status")]
        public async Task<IActionResult> SetVolunteerStatus([FromBody] SetVolunteerStatusDTO dto)
        {
            var userId = GetUserIdFromToken();

            var result = await _profileService.SetVolunteerStatusAsync(userId, dto);

            return HandleResult(result);
        }
    }
}
