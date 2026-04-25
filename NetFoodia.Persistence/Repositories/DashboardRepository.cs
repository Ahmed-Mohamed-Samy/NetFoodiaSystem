using Microsoft.EntityFrameworkCore;
using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities.DonationModule;
using NetFoodia.Persistence.Data.DbContexts;

namespace NetFoodia.Persistence.Repositories
{
    public class DashboardRepository : GenericRepository<Donation>, IDashboardRepository
    {
        private readonly NetFoodiaDbContext _dbContext;

        public DashboardRepository(NetFoodiaDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<MonthlyDonationStatsDTO>> GetDonationsPerMonthAsync(int? charityId)
        {
            var query = _dbContext.Donations.AsQueryable();

            if (charityId.HasValue)
            {
                query = query.Where(d => d.CharityId == charityId.Value);
            }

            return await query
                .GroupBy(d => d.CreatedAt.Month)
                .Select(g => new MonthlyDonationStatsDTO
                {
                    Month = g.Key,
                    Count = g.Count(),
                    Weight = g.Sum(d => d.Quantity)
                })
                .OrderBy(x => x.Month)
                .ToListAsync();
        }

        public async Task<(int totalDonations, double totalWeight)> GetTotalsAsync(int? charityId)
        {
            var query = _dbContext.Donations.AsQueryable();

            if (charityId.HasValue)
            {
                query = query.Where(d => d.CharityId == charityId.Value);
            }

            var totalDonations = await query.CountAsync();
            var totalWeight = await query.SumAsync(d => (double?)d.Quantity) ?? 0;

            return (totalDonations, totalWeight);
        }
    }
}
