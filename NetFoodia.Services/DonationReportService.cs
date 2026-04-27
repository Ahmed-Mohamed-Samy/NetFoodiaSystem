using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities.DonationModule;
using NetFoodia.Services.Specifications.DashboardSpecifications;
using NetFoodia.Services.Specifications.DonationSpecifications;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.DonationDTOs;

namespace NetFoodia.Services
{
    /// <summary>
    /// Generates donation reports grouped by <see cref="UnitType"/> (Kilos / Meals)
    /// with a nested breakdown by <see cref="FoodType"/>.
    ///
    /// All grouping and aggregation is performed in-memory after fetching the filtered
    /// donation set so the query can reuse the existing generic repository + specification
    /// infrastructure without custom SQL projections.
    /// </summary>
    public class DonationReportService : IDonationReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DonationReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc/>
        public async Task<Result<DonationUnitTypeReportDTO>> GetDonationsByUnitTypeAsync(
            int?      charityId = null,
            DateTime? from      = null,
            DateTime? to        = null)
        {
            // ── 1. Fetch ──────────────────────────────────────────────────────────
            var spec = new DonationsWithCharitySpecification(charityId);
            var all  = await _unitOfWork.GetRepository<Donation>().GetAllAsync(spec);

            // ── 2. Apply optional date-range filter in-memory ────────────────────
            IEnumerable<Donation> donations = all;

            if (from.HasValue)
                donations = donations.Where(d => d.CreatedAt >= from.Value.ToUniversalTime());

            if (to.HasValue)
                donations = donations.Where(d => d.CreatedAt <= to.Value.ToUniversalTime());

            var list = donations.ToList();

            // ── 3. Group by UnitType → then by FoodType ───────────────────────────
            var now    = DateTime.UtcNow;
            var groups = list
                .GroupBy(d => d.UnitType)
                .Select(unitGroup =>
                {
                    var byFoodType = unitGroup
                        .GroupBy(d => d.FoodType)
                        .Select(ftGroup => new FoodTypeGroupDTO
                        {
                            FoodType      = ftGroup.Key.ToString(),
                            DonationCount = ftGroup.Count(),
                            TotalQuantity = ftGroup.Sum(d => d.Quantity),
                            AverageShelfLifeRemainingHours = Math.Round(
                                ftGroup.Average(d => (d.ExpirationTime - now).TotalHours), 2)
                        })
                        .OrderBy(g => g.FoodType)
                        .ToList();

                    return new UnitTypeReportDTO
                    {
                        UnitType       = unitGroup.Key.ToString(),
                        TotalDonations = unitGroup.Count(),
                        TotalQuantity  = unitGroup.Sum(d => d.Quantity),
                        ByFoodType     = byFoodType
                    };
                })
                // Stable ordering: Kilos first, Meals second
                .OrderBy(g => g.UnitType)
                .ToList();

            // ── 4. Assemble report ────────────────────────────────────────────────
            var report = new DonationUnitTypeReportDTO
            {
                From                 = from,
                To                   = to,
                CharityId            = charityId,
                GeneratedAt          = now,
                GrandTotalDonations  = list.Count,
                GrandTotalQuantity   = list.Sum(d => d.Quantity),
                Groups               = groups
            };

            return Result<DonationUnitTypeReportDTO>.OK(report);
        }
    }
}
