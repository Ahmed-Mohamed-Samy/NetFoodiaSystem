namespace NetFoodia.Shared.CharityDTOs
{
    public class CharityListItemDTO
    {
        public int CharityId { get; set; }
        public string OrganizationName { get; set; } = default!;
        public string? City { get; set; }
        public string? Governorate { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActivated { get; set; }
    }
}
