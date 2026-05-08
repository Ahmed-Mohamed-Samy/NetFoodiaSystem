using System.Text.Json.Serialization;

namespace NetFoodia.Shared.AIMatchingDTOs
{
    /// <summary>
    /// Represents the ranked list of volunteer IDs returned by the AI matching endpoint.
    /// </summary>
    public class AIRankingResponseDTO
    {
        /// <summary>
        /// Ordered list of volunteer IDs from best to worst match.
        /// The AI service returns all submitted volunteers ranked; we take the top N.
        /// </summary>
        [JsonPropertyName("ranked_volunteer_ids")]
        public List<string> RankedVolunteerIds { get; set; } = new();
    }
}
