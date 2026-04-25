using Microsoft.AspNetCore.SignalR;
using NetFoodia.Domain.Contracts;
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
            var totalVolunteers = await _unitOfWork
                .GetRepository<VolunteerMembership>()
                .CountAsync(volunteerSpec);

            var PendingvolunteerSpec = new PendingRequestsSpecification(charityId);
            var totalPendingRequests = await _unitOfWork
                .GetRepository<VolunteerMembership>()
                .CountAsync(PendingvolunteerSpec);

            var monthlyStats = await _unitOfWork
                .DashboardRepository
                .GetDonationsPerMonthAsync(charityId);

            var (totalDonations, totalFoodWeight) = await _unitOfWork
                .DashboardRepository
                .GetTotalsAsync(charityId);

            var donationsChart = monthlyStats
                .Select(x => new ChartItemDTO
                {
                    Label = CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(x.Month),
                    Value = x.Count
                }).ToList();

            var weightChart = monthlyStats
                .Select(x => new ChartItemDTO
                {
                    Label = CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(x.Month),
                    Value = x.Weight
                }).ToList();

            return new DashboardStatsDTO
            {
                TotalDonations = totalDonations,
                TotalFoodWeight = totalFoodWeight,
                TotalVolunteers = totalVolunteers,
                PendingRequests = totalPendingRequests,
                DonationsPerMonth = donationsChart,
                FoodWeightPerMonth = weightChart
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
