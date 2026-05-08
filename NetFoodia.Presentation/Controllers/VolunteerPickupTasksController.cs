using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.DeliveryDTOs;
using NetFoodia.Shared.DonationDTOs;

namespace NetFoodia.Presentation.Controllers
{
    [Authorize(Roles = "Volunteer")]
    [Route("api/Volunteer/PickupTasks")]
    public class VolunteerPickupTasksController : ApiBaseController
    {
        private readonly IVolunteerPickupTaskService _volunteerPickupTaskService;

        public VolunteerPickupTasksController(IVolunteerPickupTaskService volunteerPickupTaskService)
        {
            _volunteerPickupTaskService = volunteerPickupTaskService;
        }

        [HttpGet("Offers")]
        public async Task<ActionResult<IEnumerable<VolunteerOfferDTO>>> ListAvailableOffers()
        {
            var userId = GetUserIdFromToken();
            var result = await _volunteerPickupTaskService.ListAvailableOffersAsync(userId);
            return HandleResult(result);
        }

        [HttpPost("Accept/{taskId:int}")]
        public async Task<ActionResult<bool>> AcceptTask(int taskId)
        {
            var userId = GetUserIdFromToken();
            var result = await _volunteerPickupTaskService.AcceptTaskAsync(userId, taskId);
            return HandleResult(result);
        }

        [HttpPost("Cancel/{taskId:int}")]
        public async Task<ActionResult<bool>> CancelAcceptedTask(int taskId)
        {
            var userId = GetUserIdFromToken();
            var result = await _volunteerPickupTaskService.CancelAcceptedTaskAsync(userId, taskId);
            return HandleResult(result);
        }

        [HttpPost("Reject/{taskId:int}")]
        public async Task<ActionResult<bool>> RejectTask(int taskId)
        {
            var userId = GetUserIdFromToken();
            var result = await _volunteerPickupTaskService.RejectTaskAsync(userId, taskId);
            return HandleResult(result);
        }

        [HttpPost("Inspect/{taskId:int}")]
        public async Task<ActionResult<bool>> InspectDonation(int taskId, InspectDonationDTO dto)
        {
            var userId = GetUserIdFromToken();
            var result = await _volunteerPickupTaskService.InspectDonationAsync(userId, taskId, dto);
            return HandleResult(result);
        }

        [HttpPost("Start/{taskId:int}")]
        public async Task<ActionResult<bool>> StartPickup(int taskId)
        {
            var userId = GetUserIdFromToken();
            var result = await _volunteerPickupTaskService.StartPickupAsync(userId, taskId);
            return HandleResult(result);
        }

        [HttpPost("Complete/{taskId:int}")]
        public async Task<ActionResult<bool>> CompleteDelivery(int taskId)
        {
            var userId = GetUserIdFromToken();
            var result = await _volunteerPickupTaskService.CompleteDeliveryAsync(userId, taskId);
            return HandleResult(result);
        }

        [HttpGet("History")]
        public async Task<ActionResult<IEnumerable<MyTaskHistoryDTO>>> GetMyTasksHistory()
        {
            var userId = GetUserIdFromToken();
            var result = await _volunteerPickupTaskService.GetMyTasksHistoryAsync(userId);
            return HandleResult(result);
        }
    }
}
