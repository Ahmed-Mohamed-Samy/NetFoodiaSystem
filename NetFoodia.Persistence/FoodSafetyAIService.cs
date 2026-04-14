using Microsoft.Extensions.ML;
using NetFoodia.Services_Abstraction;
using static NetFoodia_Web.MLModel;

namespace NetFoodia.Persistence
{
    public class FoodSafetyAIService : IFoodSafetyAIService
    {

        private readonly PredictionEnginePool<ModelInput, ModelOutput> _predictionEnginePool;
        public FoodSafetyAIService(PredictionEnginePool<ModelInput, ModelOutput> predictionEnginePool)
        {
            _predictionEnginePool = predictionEnginePool;
        }
        public async Task<FoodSafetyResult> PredictAsync(byte[] imageBytes)
        {

            var input = new ModelInput
            {
                ImageSource = imageBytes
            };

            var prediction = await Task.Run(() => _predictionEnginePool.Predict(input));

            return new FoodSafetyResult
            {
                IsSafe = prediction.PredictedLabel == "Safe",
                Confidence = prediction.Score.Max(),
                Message = (prediction.Score.Max() > 0.7) ? "High Confidence" : "Needs Manual Review"
            };
        }
    }
}
