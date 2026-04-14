using NetFoodia.Domain.Entities.InspectionModule;

namespace NetFoodia.Services.SafetyRuleEngine
{
    public class RuleEngine
    {

        private const int MaxSafeHours = 48;
        private const double MinConfidenceThreshold = 0.6;
        public SafetyStatus Evaluate(bool aiIsSafe, double confidence, DateTime donationTime)
        {


            if (!aiIsSafe)
                return SafetyStatus.Unsafe;

            var hoursSinceDonation = (DateTime.UtcNow - donationTime).TotalHours;
            if (hoursSinceDonation > MaxSafeHours)
                return SafetyStatus.Suspicious;


            if (confidence < MinConfidenceThreshold)
                return SafetyStatus.Suspicious;


            return SafetyStatus.Safe;
        }
    }
}
