using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.DonationDTOs;

namespace NetFoodia.Presentation.Controllers
{
    public class DonationController : ApiBaseController
    {
        private readonly IDonationService _donationService;

        public DonationController(IDonationService donationService)
        {
            _donationService = donationService;
        }

        [Authorize(Roles = "Donor")]
        [HttpPost("Create/{charityId:int}")]
        public async Task<ActionResult<DonationDetailsDTO>> CreateDonation(int charityId, CreateDonationDTO dto)
        {
            var donorId = GetUserIdFromToken();
            var result = await _donationService.CreateDonationAsync(donorId, charityId, dto);
            return HandleResult(result);
        }

        [Authorize(Roles = "Donor")]
        [HttpPut("Edit/{donationId:int}")]
        public async Task<IActionResult> EditDonation(int donationId, EditDonationDTO dto)
        {
            var donorId = GetUserIdFromToken();
            var result = await _donationService.EditDonationAsync(donorId, donationId, dto);
            return HandleResult(result);
        }

        [Authorize(Roles = "Donor")]
        [HttpPut("Cancel/{donationId:int}")]
        public async Task<IActionResult> CancelDonation(int donationId)
        {
            var donorId = GetUserIdFromToken();
            var result = await _donationService.CancelDonationAsync(donorId, donationId);
            return HandleResult(result);
        }

        [Authorize(Roles = "Donor")]
        [HttpGet("MyDonations")]
        public async Task<ActionResult<IEnumerable<DonationDetailsDTO>>> GetMyDonations()
        {
            var donorId = GetUserIdFromToken();
            var result = await _donationService.GetMyDonationsAsync(donorId);
            return HandleResult(result);
        }

        [Authorize(Roles = "Donor")]
        [HttpGet("Details/{donationId:int}")]
        public async Task<ActionResult<DonationDetailsDTO>> GetDonationDetails(int donationId)
        {
            var donorId = GetUserIdFromToken();
            var result = await _donationService.GetDonationDetailsAsync(donorId, donationId);
            return HandleResult(result);
        }

        [Authorize(Roles = "Donor")]
        [HttpGet("TrackStatus/{donationId:int}")]
        public async Task<ActionResult<DonationStatusDTO>> TrackDonationStatus(int donationId)
        {
            var donorId = GetUserIdFromToken();
            var result = await _donationService.TrackDonationStatusAsync(donorId, donationId);
            return HandleResult(result);
        }
    }
}