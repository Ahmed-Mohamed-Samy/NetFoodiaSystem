namespace NetFoodia.Services_Abstraction
{
    public interface IFoodSafetyAIService
    {
        Task<FoodSafetyResult> PredictAsync(byte[] imageBytes);
    }
}
