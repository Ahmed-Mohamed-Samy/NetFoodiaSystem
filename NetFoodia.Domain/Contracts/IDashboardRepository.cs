using NetFoodia.Domain.Entities.DonationModule;

namespace NetFoodia.Domain.Contracts
{
    public interface IDashboardRepository : IGenericRepository<Donation>
    {
        Task<List<MonthlyDonationStatsDTO>> GetDonationsPerMonthAsync(int? charityId);
        Task<(int totalDonations, double totalWeight)> GetTotalsAsync(int? charityId);
    }

    public class MonthlyDonationStatsDTO
    {
        public int Month { get; set; }
        public int Count { get; set; }
        public double Weight { get; set; }
    }
}
