namespace NetFoodia.Services_Abstraction
{
    public interface IRefreshTokenService
    {
        Task<string> GenerateRefreshTokenAsync(string userId);
        Task<string?> GetUserIdFromValidRefreshTokenAsync(string token);
        Task RevokeRefreshTokenAsync(string token);
    }
}
