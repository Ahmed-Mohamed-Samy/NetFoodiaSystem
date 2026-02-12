using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared;
using NetFoodia.Shared.CharityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Presentation.Controllers
{
    public class CharityController : ApiBaseController
    {
        private readonly ICharityService _charityService;

        public CharityController(ICharityService charityService)
        {
            _charityService = charityService;
        }

        [Authorize(Roles = "CharityAdmin")]
        [HttpPost("CreateMyCharity")]
        public async Task<ActionResult<CharityDetailsDTO>> CreateMyCharity(CreateMyCharityDTO dto)
        {
            var userId = GetUserIdFromToken();
            var result = await _charityService.CreateMyCharityAsync(userId, dto);
            return HandleResult(result);
        }

        [Authorize(Roles = "CharityAdmin")]
        [HttpPut("UpdateCharityInfo")]
        public async Task<IActionResult> UpdateCharityInfo(UpdateCharityInfoDTO dto)
        {
            var userId = GetUserIdFromToken();
            var result = await _charityService.UpdateCharityInfoAsync(userId, dto);
            return HandleResult(result);
        }

        [Authorize(Roles = "CharityAdmin")]
        [HttpPut("UpdateCharityLocation")]
        public async Task<IActionResult> UpdateCharityLocation(UpdateCharityLocationDTO dto)
        {
            var userId = GetUserIdFromToken();
            var result = await _charityService.UpdateCharityLocationAsync(userId, dto);
            return HandleResult(result);
        }

        [HttpGet("Details/{charityId:int}")]
        public async Task<ActionResult<CharityDetailsDTO>> Details(int charityId)
        {
            var result = await _charityService.GetCharityDetailsAsync(charityId);
            return HandleResult(result);
        }

        [HttpGet("List")]
        public async Task<ActionResult<PaginatedResult<CharityListItemDTO>>> List([FromQuery] PaginationParams pagination,
                                                                                  [FromQuery] string? search)
        {
            var result = await _charityService.ListCharitiesAsync(pagination, search);
            return HandleResult(result);
        }

    }
}
