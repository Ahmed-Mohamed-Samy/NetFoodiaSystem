using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.DonationDTOs;

namespace NetFoodia.Presentation.Controllers
{
    [Authorize(Roles = "CharityAdmin")]
    [Route("api/Charity/Donations")]
    public class CharityDonationsController : ApiBaseController
    {
        private readonly ICharityDonationService _charityDonationService;

        public CharityDonationsController(ICharityDonationService charityDonationService)
        {
            _charityDonationService = charityDonationService;
        }

        [HttpGet("Pending")]
        public async Task<ActionResult<IEnumerable<PendingDonationListItemDTO>>> ListPendingDonations()
        {
            var userId = GetUserIdFromToken();
            var result = await _charityDonationService.ListPendingDonationsAsync(userId);
            return HandleResult(result);
        }

        [HttpPost("Accept/{donationId:int}")]
        public async Task<ActionResult<bool>> AcceptDonation(int donationId)
        {
            var userId = GetUserIdFromToken();
            var result = await _charityDonationService.AcceptDonationAsync(userId, donationId);
            return HandleResult(result);
        }

        [HttpPost("Reject/{donationId:int}")]
        public async Task<ActionResult<bool>> RejectDonation(int donationId, RejectDonationDTO dto)
        {
            var userId = GetUserIdFromToken();
            var result = await _charityDonationService.RejectDonationAsync(userId, donationId, dto.Reason);
            return HandleResult(result);
        }

        [HttpPost("MarkExpired/{donationId:int}")]
        public async Task<ActionResult<bool>> MarkDonationExpired(int donationId)
        {
            var userId = GetUserIdFromToken();
            var result = await _charityDonationService.MarkDonationExpiredAsync(userId, donationId);
            return HandleResult(result);
        }

       
        [HttpGet("Accepted-Unassigned")]
        public async Task<ActionResult<IEnumerable<AcceptedUnassignedDonationDTO>>> ListAcceptedUnassigned()
        {
            var userId = GetUserIdFromToken();
            var result = await _charityDonationService.ListAcceptedUnassignedDonationsAsync(userId);
            return HandleResult(result);
        }
    }
}
