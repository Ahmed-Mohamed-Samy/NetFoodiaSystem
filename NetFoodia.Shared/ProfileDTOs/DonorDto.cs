namespace NetFoodia.Shared.ProfileDTOs
{
    public class DonorDto
    {
        public string UserId { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public bool IsBusiness { get; set; }
        public string? BusinessType { get; set; }
        public string Address { get; set; } = default!;
        public bool IsVerified { get; set; }
        public decimal ReliabilityScore { get; set; }
    }
}
