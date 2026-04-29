using NetFoodia.Domain.Entities.DonationModule;
using NetFoodia.Services.Specifications;
using NetFoodia.Shared.DonationDTOs;

namespace NetFoodia.Services.Specifications.DonationSpecifications
{
    /// <summary>
    /// Specification used exclusively by the donation reporting pipeline.
    /// All filter criteria are pushed down to the database via EF Core's
    /// <c>WHERE</c> clause — no in-memory filtering required.
    ///
    /// Every parameter is optional. When <c>null</c>, the corresponding
    /// predicate is skipped and the column is not included in the SQL filter.
    /// </summary>
    public class DonationReportSpecification : BaseSpecification<Donation>
    {
        /// <summary>
        /// Creates a new specification with all supported filter axes.
        /// </summary>
        /// <param name="charityId">Restrict to a single charity.</param>
        /// <param name="from">Inclusive lower bound on <c>CreatedAt</c> (UTC).</param>
        /// <param name="to">Inclusive upper bound on <c>CreatedAt</c> (UTC).</param>
        /// <param name="unitType">Restrict to a single <see cref="UnitType"/> (Kilos or Meals).</param>
        /// <param name="foodType">Restrict to a single <see cref="FoodType"/> category.</param>
        public DonationReportSpecification(
            int?      charityId = null,
            DateTime? from      = null,
            DateTime? to        = null,
            UnitType? unitType  = null,
            FoodType? foodType  = null)
            : base(d =>
                // ── Charity filter ───────────────────────────────────────────────
                (!charityId.HasValue || d.CharityId == charityId.Value) &&

                // ── Date-range filter (converted to UTC at the call-site) ────────
                (!from.HasValue     || d.CreatedAt  >= from.Value) &&
                (!to.HasValue       || d.CreatedAt  <= to.Value)   &&

                // ── UnitType filter ──────────────────────────────────────────────
                (!unitType.HasValue  || d.UnitType  == unitType.Value) &&

                // ── FoodType filter ──────────────────────────────────────────────
                (!foodType.HasValue  || d.FoodType  == foodType.Value))
        {
            // Eager-load Charity so OrganizationName is available if the caller needs it.
            AddInclude(d => d.Charity);

            // Most-urgent donations first (highest urgency score = least time remaining).
            AddOrderByDesc(d => d.UrgencyScore);
        }
    }
}
