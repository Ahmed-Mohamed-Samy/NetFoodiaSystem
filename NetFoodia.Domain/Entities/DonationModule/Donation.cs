using NetFoodia.Domain.Entities.CharityModule;
using NetFoodia.Domain.Entities.InspectionModule;
using NetFoodia.Domain.Entities.ProfileModule;
using NetFoodia.Domain.Entities.SharedValueObjects;
using NetFoodia.Domain.Events;
using NetFoodia.Shared.DonationDTOs;

namespace NetFoodia.Domain.Entities.DonationModule
{
    public class Donation : BaseEntity
    {
        public string DonorId { get; set; } = default!;
        public DonorProfile Donor { get; set; } = default!;

        public int CharityId { get; set; }
        public Charity Charity { get; set; } = default!;

        /// <summary>Strongly-typed food category — drives expiry calculation and reporting.</summary>
        public FoodType FoodType { get; set; }

        /// <summary>
        /// Measurement unit for <see cref="Quantity"/>.
        /// Defaults via <see cref="FoodExpiryPolicy.ResolveDefaultUnitType"/> but can be overridden by the donor.
        /// </summary>
        public UnitType UnitType { get; set; }

        public int Quantity { get; set; }
        public DateTime PreparedTime { get; set; }

        /// <summary>
        /// Calculated automatically from <see cref="FoodType"/> + <see cref="PreparedTime"/>
        /// using <see cref="FoodExpiryPolicy.CalculateExpiry"/>.
        /// Donors cannot set this manually — it is derived.
        /// </summary>
        public DateTime ExpirationTime { get; set; }

        public GeoLocation PickupLocation { get; set; } = default!;
        public string? Notes { get; set; }
        public float UrgencyScore { get; set; }
        public DonationStatus Status { get; set; } = DonationStatus.Pending;
        public DateTime? AcceptedAt { get; set; }
        public DateTime? AssignedAt { get; set; }
        public DateTime? PickedUpAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string ImagePath { get; set; } = default!;
        public FoodInspection? Inspection { get; set; }
        //public string? RejectionReason { get; set; }
        //public DateTime? ReviewedAt { get; set; }

        /// <summary>
        /// Derives <see cref="ExpirationTime"/> from <see cref="FoodType"/> and <see cref="PreparedTime"/>
        /// and resolves <see cref="UnitType"/> when the caller has not explicitly set it.
        /// Call this once after all fields are assigned.
        /// </summary>
        public void ApplyFoodPolicy()
        {
            ExpirationTime = FoodExpiryPolicy.CalculateExpiry(FoodType, PreparedTime);

            // Only overwrite UnitType if it was left at the default (0 / unset)
            if (UnitType == 0)
                UnitType = FoodExpiryPolicy.ResolveDefaultUnitType(FoodType);
        }

        public void MarkAsCreated()
        {
            this.AddDomainEvent(new DonationCreatedEvent(this));
        }
    }
}
