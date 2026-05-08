using System.Text.Json.Serialization;

namespace NetFoodia.Shared.AIMatchingDTOs
{
    /// <summary>
    /// Represents a single volunteer candidate sent to the AI ranking endpoint.
    /// Field names match the Python model's expected JSON schema exactly.
    /// </summary>
    public class VolunteerRankingRequestItemDTO
    {
        [JsonPropertyName("volunteer_id")]
        public string VolunteerId { get; set; } = default!;

        /// <summary>Distance (km) from the volunteer's current location to the donation pickup point.</summary>
        [JsonPropertyName("distance_to_pickup")]
        public double DistanceToPickup { get; set; }

        /// <summary>Estimated distance (km) of the delivery task itself (pickup → drop-off).</summary>
        [JsonPropertyName("distance_task")]
        public double DistanceTask { get; set; }

        /// <summary>Vehicle type encoded as integer: 0 = None/Walk, 1 = Bicycle, 2 = Motorbike, 3 = Car, 4 = Van.</summary>
        [JsonPropertyName("vehicle")]
        public int Vehicle { get; set; }
    }
}
