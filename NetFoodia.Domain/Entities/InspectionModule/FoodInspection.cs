using NetFoodia.Domain.Entities.DonationModule;

namespace NetFoodia.Domain.Entities.InspectionModule
{
    public class FoodInspection : BaseEntity
    {

        public int DonationId { get; set; }
        public Donation Donation { get; set; } = default!;
        public SafetyStatus SafetyStatus { get; set; }
        public double RiskScore { get; set; }          // 0.0 → 1.0
        public string? Notes { get; set; }
        public bool? AiIsSafe { get; set; }
        public double? AiConfidence { get; set; }
        public InspectionSource Source { get; set; }
        public string? ReviewedByAdminId { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
