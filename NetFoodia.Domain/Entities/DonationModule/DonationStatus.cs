namespace NetFoodia.Domain.Entities.DonationModule
{
    /// <summary>
    /// Represents every valid state in the Donation Lifecycle.
    ///
    /// State machine (happy path):
    ///   Pending → Accepted → InspectionPending → ReadyForPickup → InTransit → Completed
    ///
    /// Terminal off-ramps:
    ///   Any state → Cancelled   (donor cancels before pickup starts)
    ///   Pending   → Rejected    (charity rejects before accepting)
    ///   InspectionPending → Rejected  (volunteer inspector rejects)
    ///   Any active state → Expired    (system/charity marks as expired)
    /// </summary>
    public enum DonationStatus
    {
        /// <summary>Donation submitted by donor, awaiting charity review.</summary>
        Pending           = 1,

        /// <summary>Charity accepted the donation — looking for a volunteer.</summary>
        Accepted          = 2,

        /// <summary>Charity rejected the donation — terminal state.</summary>
        Rejected          = 3,

        /// <summary>Donor cancelled the donation — terminal state.</summary>
        Cancelled         = 4,

        /// <summary>Food is past its expiry time — terminal state.</summary>
        Expired           = 5,

        /// <summary>
        /// Volunteer has been assigned and is physically inspecting the food
        /// before transporting it.
        /// </summary>
        InspectionPending = 6,

        /// <summary>
        /// Volunteer inspection passed — food cleared for pickup and transport.
        /// </summary>
        ReadyForPickup    = 7,

        /// <summary>
        /// Volunteer has picked up the food and is currently transporting it
        /// to the charity.
        /// </summary>
        InTransit         = 8,

        /// <summary>
        /// Charity confirmed receipt of the donation — terminal happy-path state.
        /// </summary>
        Completed         = 9
    }
}
