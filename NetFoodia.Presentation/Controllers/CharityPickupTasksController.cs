using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.DeliveryDTOs;

namespace NetFoodia.Presentation.Controllers
{
    [Authorize(Roles = "CharityAdmin")]
    [Route("api/Charity/PickupTasks")]
    public class CharityPickupTasksController : ApiBaseController
    {
        private readonly ICharityPickupTaskService _charityPickupTaskService;

        public CharityPickupTasksController(ICharityPickupTaskService charityPickupTaskService)
        {
            _charityPickupTaskService = charityPickupTaskService;
        }

        [HttpPost("Create/{donationId:int}")]
        public async Task<ActionResult<PickupTaskDetailsDTO>> CreatePickupTask(int donationId, CreatePickupTaskDTO dto)
        {
            var userId = GetUserIdFromToken();
            var result = await _charityPickupTaskService.CreatePickupTaskAsync(userId, donationId, dto);
            return HandleResult(result);
        }

        [HttpGet("Open")]
        public async Task<ActionResult<IEnumerable<OpenTaskListItemDTO>>> ListOpenTasks()
        {
            var userId = GetUserIdFromToken();
            var result = await _charityPickupTaskService.ListOpenTasksAsync(userId);
            return HandleResult(result);
        }

        [HttpPost("Offer/{taskId:int}")]
        public async Task<ActionResult<bool>> OfferTask(int taskId, OfferTaskDTO dto)
        {
            var userId = GetUserIdFromToken();
            var result = await _charityPickupTaskService.OfferTaskToVolunteerAsync(userId, taskId, dto.VolunteerUserId);
            return HandleResult(result);
        }

        [HttpPost("Assign/{taskId:int}")]
        public async Task<ActionResult<bool>> AssignTask(int taskId, AssignTaskDTO dto)
        {
            var userId = GetUserIdFromToken();
            var result = await _charityPickupTaskService.AssignTaskAsync(userId, taskId, dto.VolunteerUserId);
            return HandleResult(result);
        }

        [HttpPost("Reassign/{taskId:int}")]
        public async Task<ActionResult<bool>> ReassignTask(int taskId)
        {
            var userId = GetUserIdFromToken();
            var result = await _charityPickupTaskService.ReassignTaskAsync(userId, taskId);
            return HandleResult(result);
        }
    }
}
