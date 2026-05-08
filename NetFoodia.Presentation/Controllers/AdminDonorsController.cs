using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.ProfileDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetFoodia.Presentation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDonorsController : ApiBaseController
    {
        private readonly IAdminDonorService _adminDonorService;

        public AdminDonorsController(IAdminDonorService adminDonorService)
        {
            _adminDonorService = adminDonorService;
        }

        [HttpGet("unverified-business")]
        public async Task<ActionResult<IReadOnlyList<DonorDto>>> GetUnverifiedBusinessDonors()
        {
            var result = await _adminDonorService.GetUnverifiedBusinessDonorsAsync();
            return HandleResult(result);
        }

        [HttpPut("{id}/verify")]
        public async Task<IActionResult> VerifyDonor(string id)
        {
            var result = await _adminDonorService.VerifyDonorAsync(id);
            return HandleResult(result);
        }

        [HttpPut("{id}/unverify")]
        public async Task<IActionResult> UnverifyDonor(string id)
        {
            var result = await _adminDonorService.UnverifyDonorAsync(id);
            return HandleResult(result);
        }
    }
}
