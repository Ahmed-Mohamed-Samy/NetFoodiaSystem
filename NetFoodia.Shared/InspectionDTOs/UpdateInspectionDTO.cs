namespace NetFoodia.Shared.InspectionDTOs
{
    public class UpdateInspectionDTO
    {
        public SafetyStatus SafetyStatus { get; set; }
        public double? RiskScore { get; set; }
        public string? Notes { get; set; }
    }
}
