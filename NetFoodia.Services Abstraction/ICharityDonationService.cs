using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.DonationDTOs;

namespace NetFoodia.Services_Abstraction
{
    public interface ICharityDonationService
    {
        Task<Result<IEnumerable<PendingDonationListItemDTO>>> ListPendingDonationsAsync(string charityAdminUserId);
        Task<Result<bool>> AcceptDonationAsync(string charityAdminUserId, int donationId);
        Task<Result<bool>> RejectDonationAsync(string charityAdminUserId, int donationId, string reason);
        Task<Result<bool>> MarkDonationExpiredAsync(string charityAdminUserId, int donationId);
        Task<Result<IEnumerable<AcceptedUnassignedDonationDTO>>> ListAcceptedUnassignedDonationsAsync(string charityAdminUserId);
    }
}