namespace NetFoodia.Shared.AuthenticationDTOs
{
    public class RegisterDTO
    {
        public string FullName { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public UserRole Role { get; set; }
    }
}
