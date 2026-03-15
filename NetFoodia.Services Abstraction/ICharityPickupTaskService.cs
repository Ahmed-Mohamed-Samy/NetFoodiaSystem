using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.DeliveryDTOs;

namespace NetFoodia.Services_Abstraction
{
    public interface ICharityPickupTaskService
    {
        Task<Result<PickupTaskDetailsDTO>> CreatePickupTaskAsync(string charityAdminUserId, int donationId, CreatePickupTaskDTO dto);
        Task<Result<IEnumerable<OpenTaskListItemDTO>>> ListOpenTasksAsync(string charityAdminUserId);
        Task<Result<bool>> OfferTaskToVolunteerAsync(string charityAdminUserId, int taskId, string volunteerUserId);
        Task<Result<bool>> AssignTaskAsync(string charityAdminUserId, int taskId, string volunteerUserId);
        Task<Result<bool>> ReassignTaskAsync(string charityAdminUserId, int taskId);
    }
}
