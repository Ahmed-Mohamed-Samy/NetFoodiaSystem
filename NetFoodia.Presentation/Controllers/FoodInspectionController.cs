using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared;
using NetFoodia.Shared.InspectionDTOs;

namespace NetFoodia.Presentation.Controllers
{
    [Authorize]
    public class FoodInspectionController : ApiBaseController
    {
        private readonly IFoodInspectionService _foodInspectionService;

        public FoodInspectionController(IFoodInspectionService foodInspectionService)
        {
            _foodInspectionService = foodInspectionService;
        }


        //[HttpPost("scan/{donationId}")]
        //public async Task<ActionResult<bool>> ScanDonation(int donationId)
        //{
        //    var result = await _foodInspectionService.CreateOrUpdateFromAI(donationId);

        //    return HandleResult(result);
        //}

        [HttpGet("all-inspections")]
        [Authorize(Roles = "CharityAdmin")]
        public async Task<ActionResult<PaginatedResult<FoodInspectionDTO>>> GetAll([FromQuery] PaginationParams @params, [FromQuery] string? status = null)
        {
            var result = await _foodInspectionService.GetAllInspectionsPaginated(@params.PageIndex, @params.PageSize, status);

            return HandleResult(result);
        }

        [HttpPut("manual-update/{inspectionId}")]
        [Authorize(Roles = "CharityAdmin")]
        public async Task<ActionResult<bool>> ManualUpdate(int inspectionId, [FromBody] UpdateInspectionDTO dto)
        {

            var adminId = GetUserIdFromToken();

            var result = await _foodInspectionService.UpdateManual(inspectionId, dto, adminId);

            return HandleResult(result);
        }


        [HttpGet("donation/{donationId}")]
        [Authorize(Roles = "CharityAdmin,Donor")]
        public async Task<ActionResult<FoodInspectionDTO?>> GetByDonation(int donationId)
        {
            var result = await _foodInspectionService.GetByDonation(donationId);

            return HandleResult(result);
        }

        [HttpGet("suspicious")]
        [Authorize(Roles = "CharityAdmin")]
        public async Task<ActionResult<IEnumerable<FoodInspectionDTO>>> GetSuspicious()
        {
            var result = await _foodInspectionService.GetSuspiciousInspections();
            return HandleResult(result);
        }
    }
}
