namespace NetFoodia.Services_Abstraction
{
    public class FoodSafetyResult
    {
        public bool IsSafe { get; set; } = default!;
        public double Confidence { get; set; }
        public string Message { get; set; }

    }
}