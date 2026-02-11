using NetFoodia.Shared.AuthenticationDTOs;
using NetFoodia.Shared.CommonResult;

namespace NetFoodia.Services_Abstraction
{
    public interface IAuthenticationService
    {
        Task<Result<TokenResponseDTO>> RegisterAsync(RegisterDTO registerDTO);
        Task<Result<TokenResponseDTO>> LoginAsync(LoginDTO loginDTO);
        Task<Result<TokenResponseDTO>> RefreshTokenAsync(RefreshRequestDTO requestDTO);
        Task<Result> LogoutAsync(RefreshRequestDTO requestDTO);
        Task<Result> ChangePasswordAsync(string userEmail, ChangePasswordDTO passwordDTO);
        Task<bool> EmailExistsAsync(string email);
    }
}
