using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities.DonationModule;
using NetFoodia.Services.Specifications.DonationSpecifications;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.DonationDTOs;
using DonationStatus = NetFoodia.Domain.Entities.DonationModule.DonationStatus;

namespace NetFoodia.Services
{
    /// <summary>
    /// Generates donation reports grouped by <see cref="UnitType"/> (Kilos / Meals)
    /// with a nested breakdown by <see cref="FoodType"/>.
    ///
    /// All filtering — charity, date-range, UnitType, FoodType — is pushed to the
    /// database via <see cref="DonationReportSpecification"/>. No in-memory filtering.
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
            DateTime? to        = null,
            UnitType? unitType  = null,
            FoodType? foodType  = null)
        {
            // ── 1. Normalise date bounds to UTC ───────────────────────────────────
            var fromUtc = from?.ToUniversalTime();
            var toUtc   = to?.ToUniversalTime();

            // ── 2. DB query — all filters resolved by EF Core inside SQL WHERE ────
            var spec = new DonationReportSpecification(
                charityId: charityId,
                from:      fromUtc,
                to:        toUtc,
                unitType:  unitType,
                foodType:  foodType);

            var list = (await _unitOfWork.GetRepository<Donation>().GetAllAsync(spec)).ToList();

            // ── 3. In-memory grouping (no filtering here — data already scoped) ───
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
                            AverageShelfLifeRemainingHours = ftGroup.Any(d => d.ExpirationTime > now)
                                ? Math.Round(ftGroup.Where(d => d.ExpirationTime > now).Average(d => (d.ExpirationTime - now).TotalHours), 2)
                                : 0,
                            ExpiredCount = ftGroup.Count(d => d.ExpirationTime < now || d.Status == DonationStatus.Expired)
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
                .OrderBy(g => g.UnitType)   // Stable: Kilos before Meals
                .ToList();

            // ── 4. Assemble report ────────────────────────────────────────────────
            var report = new DonationUnitTypeReportDTO
            {
                From                    = from,
                To                      = to,
                CharityId               = charityId,
                AppliedUnitTypeFilter   = unitType?.ToString(),
                AppliedFoodTypeFilter   = foodType?.ToString(),
                GeneratedAt             = now,
                GrandTotalDonations     = list.Count,
                GrandTotalQuantity      = list.Sum(d => d.Quantity),
                Groups                  = groups
            };

            return Result<DonationUnitTypeReportDTO>.OK(report);
        }
    }
}
