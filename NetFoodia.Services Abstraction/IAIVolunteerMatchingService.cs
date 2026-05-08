using NetFoodia.Shared.AIMatchingDTOs;

namespace NetFoodia.Services_Abstraction
{
    /// <summary>
    /// Client interface for the external AI Smart Matching service.
    /// Implementations POST volunteer candidates to the ranking endpoint
    /// and return the ordered list of top volunteer IDs.
    /// </summary>
    public interface IAIVolunteerMatchingService
    {
        /// <summary>
        /// Sends a list of volunteer candidates to the AI ranking endpoint and
        /// returns the volunteer IDs ordered from best to worst match.
        /// </summary>
        /// <param name="candidates">Volunteer feature vectors to rank.</param>
        /// <returns>Ordered list of volunteer IDs (best first), or empty list on failure.</returns>
        Task<List<string>> RankVolunteersAsync(List<VolunteerRankingRequestItemDTO> candidates);
    }
}
