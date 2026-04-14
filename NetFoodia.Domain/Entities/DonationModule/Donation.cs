using NetFoodia.Domain.Entities.CharityModule;
using NetFoodia.Domain.Entities.InspectionModule;
using NetFoodia.Domain.Entities.ProfileModule;
using NetFoodia.Domain.Entities.SharedValueObjects;
using NetFoodia.Domain.Events;

namespace NetFoodia.Domain.Entities.DonationModule
{
    public class Donation : BaseEntity
    {
        public string DonorId { get; set; } = default!;
        public DonorProfile Donor { get; set; } = default!;

        public int CharityId { get; set; }
        public Charity Charity { get; set; } = default!;

        public string FoodType { get; set; } = default!;
        public int Quantity { get; set; }
        public DateTime PreparedTime { get; set; }
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


        public void MarkAsCreated()
        {
            this.AddDomainEvent(new DonationCreatedEvent(this));
        }
    }
}
