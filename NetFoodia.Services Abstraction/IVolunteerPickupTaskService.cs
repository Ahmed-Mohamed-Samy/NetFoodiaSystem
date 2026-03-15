using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.DeliveryDTOs;

namespace NetFoodia.Services_Abstraction
{
    public interface IVolunteerPickupTaskService
    {
        Task<Result<IEnumerable<VolunteerOfferDTO>>> ListAvailableOffersAsync(string volunteerUserId);
        Task<Result<bool>> AcceptTaskAsync(string volunteerUserId, int taskId);
        Task<Result<bool>> RejectTaskAsync(string volunteerUserId, int taskId);
        Task<Result<bool>> StartPickupAsync(string volunteerUserId, int taskId);
        Task<Result<bool>> CompleteDeliveryAsync(string volunteerUserId, int taskId);
        Task<Result<IEnumerable<MyTaskHistoryDTO>>> GetMyTasksHistoryAsync(string volunteerUserId);
        Task<Result<bool>> CancelAcceptedTaskAsync(string volunteerUserId, int taskId);
    }
}
