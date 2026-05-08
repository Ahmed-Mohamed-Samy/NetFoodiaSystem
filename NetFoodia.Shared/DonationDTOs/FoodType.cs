namespace NetFoodia.Shared.DonationDTOs
{
    /// <summary>
    /// Classifies the type of donated food.
    /// Each category carries a different default shelf-life used by the expiry policy.
    /// </summary>
    public enum FoodType
    {
        /// <summary>Cooked or hot meals — shortest shelf life (4 hours).</summary>
        CookedMeal = 1,

        /// <summary>Perishable raw ingredients such as meat, dairy, or fresh produce (24 hours).</summary>
        Perishable = 2,

        /// <summary>Baked goods — bread, pastries, cakes (48 hours).</summary>
        BakedGoods = 3,

        /// <summary>Dry / canned / packaged goods — longest shelf life (168 hours / 7 days).</summary>
        NonPerishable = 4,

        /// <summary>Beverages including juice, water, soft drinks (72 hours).</summary>
        Beverage = 5,

        /// <summary>Frozen items that are transported while still frozen (720 hours / 30 days).</summary>
        Frozen = 6
    }
}
