using Microsoft.Extensions.Logging;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.AIMatchingDTOs;
using System.Net.Http.Json;
using System.Text.Json;

namespace NetFoodia.Services
{
    /// <summary>
    /// HTTP client implementation that calls the deployed AI Smart Matching microservice
    /// at https://ai-service-production-507e.up.railway.app/rank-volunteers.
    /// </summary>
    public class AIVolunteerMatchingService : IAIVolunteerMatchingService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AIVolunteerMatchingService> _logger;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        public AIVolunteerMatchingService(
            HttpClient httpClient,
            ILogger<AIVolunteerMatchingService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<List<string>> RankVolunteersAsync(List<VolunteerRankingRequestItemDTO> candidates)
        {
            if (candidates is null || candidates.Count == 0)
                return new List<string>();

            try
            {
                _logger.LogInformation(
                    "Calling AI ranking endpoint with {Count} volunteer candidates.", candidates.Count);

                var response = await _httpClient.PostAsJsonAsync("rank-volunteers", candidates, _jsonOptions);

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning(
                        "AI ranking endpoint returned {StatusCode}. Body: {Body}",
                        (int)response.StatusCode, body);
                    return new List<string>();
                }

                // Read raw body first so we can log it if deserialization fails
                var rawBody = await response.Content.ReadAsStringAsync();

                _logger.LogDebug("AI ranking raw response: {Body}", rawBody);

                var ranked = JsonSerializer.Deserialize<List<AIRankedVolunteerDTO>>(rawBody, _jsonOptions);

                if (ranked is null || ranked.Count == 0)
                {
                    _logger.LogWarning("AI ranking endpoint returned an empty or null response. Body: {Body}", rawBody);
                    return new List<string>();
                }

                _logger.LogInformation(
                    "AI returned {Count} ranked volunteers. Top volunteer: {TopId} (score: {Score})",
                    ranked.Count,
                    ranked[0].VolunteerId,
                    ranked[0].Score?.ToString("F4") ?? "N/A");

                return ranked.Select(r => r.VolunteerId).ToList();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error while calling AI ranking endpoint.");
                return new List<string>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while calling AI ranking endpoint.");
                return new List<string>();
            }
        }
    }
}
