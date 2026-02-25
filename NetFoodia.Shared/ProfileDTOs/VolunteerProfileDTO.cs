namespace NetFoodia.Shared.ProfileDTOs
{
    public class VolunteerProfileDTO
    {
        public string Status { get; set; } = default!;
        public string? VehicleType { get; set; }
        public DateTime LastActiveAt { get; set; }
        public GeoLocationDTO? Location { get; set; }
        public string Address { get; set; } = default!;
    }
}
