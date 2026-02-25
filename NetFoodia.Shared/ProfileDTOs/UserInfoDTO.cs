namespace NetFoodia.Shared.ProfileDTOs
{
    public class UserInfoDTO
    {
        public string UserId { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
