namespace NetFoodia.Shared.ProfileDTOs
{
    public class UpsertDonorProfileDTO
    {
        public bool IsBusiness { get; set; }
        public string? BusinessType { get; set; }
        public GeoLocationDTO Location { get; set; } = default!;
        public string Address { get; set; } = default!;
    }
}
