namespace NetFoodia.Shared.InspectionDTOs
{
    public class FoodInspectionDTO
    {
        public int Id { get; set; }
        public int DonationId { get; set; }
        public string SafetyStatus { get; set; } = default!;
        public float RiskScore { get; set; }
        public bool AiIsSafe { get; set; }
        public double AiConfidence { get; set; }
        public string Source { get; set; } = default!;
        public string? Notes { get; set; }
        public string FoodType { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
    }
}
