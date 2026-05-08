namespace NetFoodia.Shared.ProfileDTOs
{
    public class UpsertVolunteerProfileDTO
    {
        public GeoLocationDTO Location { get; set; } = default!;
        public string Address { get; set; } = default!;
        public VehicleType VehicleType { get; set; }
    }
}
