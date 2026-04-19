using Microsoft.AspNetCore.SignalR;
using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities.DonationModule;
using NetFoodia.Domain.Entities.MembershipModule;
using NetFoodia.Services.Hubs;
using NetFoodia.Services.Specifications.DashboardSpecifications;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.DashboardDTOs;
using System.Globalization;

namespace NetFoodia.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<DashboardHub> _hubContext;

        public DashboardService(IUnitOfWork unitOfWork, IHubContext<DashboardHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        public async Task<Result<DashboardStatsDTO>> GetStatsAsync(int? charityId = null)
        {

            var volunteerSpec = new VolunteersByCharitySpecification(charityId);
            var totalVolunteers = await _unitOfWork.GetRepository<VolunteerMembership>().CountAsync(volunteerSpec);


            var donationSpec = new DonationsWithCharitySpecification(charityId);
            var donations = await _unitOfWork.GetRepository<Donation>().GetAllAsync(donationSpec);

            return new DashboardStatsDTO
            {
                TotalDonations = donations.Count(),
                TotalFoodWeight = donations.Sum(d => d.Quantity),
                TotalVolunteers = totalVolunteers,

                MonthlyAnalysis = donations
                    .GroupBy(d => d.CreatedAt.Month)
                    .Select(g => new ChartItemDTO
                    {
                        Label = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key),
                        Value = g.Count()
                    }).OrderBy(x => x.Label).ToList()
            };
        }

        public async Task<Result> SendRealTimeUpdate(int charityId)
        {

            var charityStats = await GetStatsAsync(charityId);
            await _hubContext.Clients.Group($"Charity_{charityId}").SendAsync("ReceiveUpdate", charityStats);


            var globalStats = await GetStatsAsync();
            await _hubContext.Clients.Group("SuperAdminGroup").SendAsync("ReceiveGlobalUpdate", globalStats);

            return Result.OK();
        }
    }
}
