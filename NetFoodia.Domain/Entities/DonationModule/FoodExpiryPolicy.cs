using NetFoodia.Shared.DonationDTOs;

namespace NetFoodia.Domain.Entities.DonationModule
{
    /// <summary>
    /// Pure domain service that encapsulates expiry calculation rules per <see cref="FoodType"/>.
    /// Contains no I/O — safe to unit-test without any infrastructure dependency.
    /// </summary>
    public static class FoodExpiryPolicy
    {
        // Default shelf-life windows per food category (in hours from PreparedTime)
        private static readonly Dictionary<FoodType, double> _shelfLifeHours = new()
        {
            { FoodType.CookedMeal,    4   },   // hot/cooked food spoils fastest
            { FoodType.Perishable,    24  },   // raw meat, dairy, fresh produce
            { FoodType.BakedGoods,    48  },   // bread, pastries
            { FoodType.Beverage,      72  },   // juices, soft drinks
            { FoodType.NonPerishable, 168 },   // canned / dry / packaged — 7 days
            { FoodType.Frozen,        720 },   // frozen items — 30 days
        };

        /// <summary>
        /// Calculates the expiry date-time based on food category and when the food was prepared.
        /// </summary>
        /// <param name="foodType">The category of the donated food.</param>
        /// <param name="preparedTime">UTC timestamp when the food was prepared or packaged.</param>
        /// <returns>UTC expiry date-time.</returns>
        public static DateTime CalculateExpiry(FoodType foodType, DateTime preparedTime)
        {
            if (!_shelfLifeHours.TryGetValue(foodType, out var hours))
                throw new ArgumentOutOfRangeException(nameof(foodType),
                    $"No expiry rule is defined for FoodType '{foodType}'.");

            return preparedTime.ToUniversalTime().AddHours(hours);
        }

        /// <summary>
        /// Returns the shelf-life window (in hours) for the given <paramref name="foodType"/>.
        /// </summary>
        public static double GetShelfLifeHours(FoodType foodType)
        {
            if (!_shelfLifeHours.TryGetValue(foodType, out var hours))
                throw new ArgumentOutOfRangeException(nameof(foodType),
                    $"No shelf-life rule is defined for FoodType '{foodType}'.");

            return hours;
        }

        /// <summary>
        /// Returns the canonical <see cref="UnitType"/> that best represents the given <paramref name="foodType"/>.
        /// Cooked meals are counted as Meals; everything else defaults to Kilos.
        /// </summary>
        public static UnitType ResolveDefaultUnitType(FoodType foodType)
            => foodType == FoodType.CookedMeal ? UnitType.Meals : UnitType.Kilos;
    }
}
