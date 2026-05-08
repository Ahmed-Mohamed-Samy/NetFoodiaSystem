using System.Text.Json.Serialization;

namespace NetFoodia.Shared.AIMatchingDTOs
{
    /// <summary>
    /// Represents a single ranked volunteer entry returned by the AI endpoint.
    /// The endpoint returns an array of these objects ordered best → worst.
    /// </summary>
    public class AIRankedVolunteerDTO
    {
        [JsonPropertyName("volunteer_id")]
        public string VolunteerId { get; set; } = default!;

        /// <summary>Optional ranking score — logged for diagnostics but not used for selection.</summary>
        [JsonPropertyName("score")]
        public double? Score { get; set; }
    }
}
