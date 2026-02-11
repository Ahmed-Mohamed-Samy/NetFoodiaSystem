namespace NetFoodia.Shared.AuthenticationDTOs
{
    public class TokenResponseDTO
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
