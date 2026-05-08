namespace NetFoodia.Shared.DonationDTOs
{
    /// <summary>
    /// Represents the unit of measurement for a donation quantity.
    /// Used by the reporting service to group and aggregate donations.
    /// </summary>
    public enum UnitType
    {
        /// <summary>Quantity is expressed in kilograms — typically raw / packaged food.</summary>
        Kilos = 1,

        /// <summary>Quantity is expressed as individual meal portions — typically cooked food.</summary>
        Meals = 2
    }
}
