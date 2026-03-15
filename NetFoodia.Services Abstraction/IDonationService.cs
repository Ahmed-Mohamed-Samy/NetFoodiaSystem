using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.DonationDTOs;

namespace NetFoodia.Services_Abstraction
{
    public interface IDonationService
    {
        Task<Result<DonationDetailsDTO>> CreateDonationAsync(string donorId, int charityId, CreateDonationDTO dto);
        Task<Result> EditDonationAsync(string donorId, int donationId, EditDonationDTO dto);
        Task<Result> CancelDonationAsync(string donorId, int donationId);
        Task<Result<IEnumerable<DonationListItemDTO>>> GetMyDonationsAsync(string donorId);
        Task<Result<DonationDetailsDTO>> GetDonationDetailsAsync(string donorId, int donationId);
        Task<Result<DonationStatusDTO>> TrackDonationStatusAsync(string donorId, int donationId);
    }
}
