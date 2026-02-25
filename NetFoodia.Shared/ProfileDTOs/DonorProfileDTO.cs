namespace NetFoodia.Shared.ProfileDTOs
{
    public class DonorProfileDTO
    {
        public bool IsBusiness { get; set; }
        public string? BusinessType { get; set; }
        public bool IsVerified { get; set; }
        public float ReliabilityScore { get; set; }
        public GeoLocationDTO? Location { get; set; }
        public string Address { get; set; }

    }
}
