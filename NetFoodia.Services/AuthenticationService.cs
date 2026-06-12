using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NetFoodia.Domain.Entities.IdentityModule;
using NetFoodia.Services.Security;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.AuthenticationDTOs;
using NetFoodia.Shared.CommonResult;
using System.Data;

namespace NetFoodia.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IJwtService _jwtService;
        private readonly IMemoryCache _memoryCache;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(UserManager<ApplicationUser> userManager,
            IRefreshTokenService refreshTokenService, IJwtService jwtService,
            IMemoryCache memoryCache, IEmailService emailService, ILogger<AuthenticationService> logger
            )
        {
            _userManager = userManager;
            _refreshTokenService = refreshTokenService;
            _jwtService = jwtService;
            _memoryCache = memoryCache;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<Result<TokenResponseDTO>> RegisterAsync(RegisterDTO registerDTO)
        {

            var exists = await _userManager.FindByEmailAsync(registerDTO.Email);
            if (exists is not null) return Error.Validation(description: "Email already exists");

            var user = new ApplicationUser
            {
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                FullName = registerDTO.FullName,
                PhoneNumber = registerDTO.Phone,
                Role = registerDTO.Role.ToString(),
            };

            var identityResult = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!identityResult.Succeeded)
                return identityResult.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();

            var roleResult = await _userManager.AddToRoleAsync(user, registerDTO.Role.ToString());
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return Error.Failure("RoleAssignmentFailed", "User Created But Role Assignment Failed");
            }

            var accessToken = await _jwtService.GenerateTokenAsync(user);
            var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user.Id);

            return new TokenResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public async Task<Result<TokenResponseDTO>> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user is null)
                return Error.InvalidCredentials("User.InvalidCrendentials");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!isPasswordValid)
                return Error.InvalidCredentials("User.InvalidCrendentials");

            var accessToken = await _jwtService.GenerateTokenAsync(user);
            var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user.Id);

            return new TokenResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public async Task<Result<TokenResponseDTO>> RefreshTokenAsync(RefreshRequestDTO requestDTO)
        {
            var userId = await _refreshTokenService.GetUserIdFromValidRefreshTokenAsync(requestDTO.RefreshToken);

            if (userId is null)
                return Error.InvalidCredentials("User.InvalidCrendentials", "Refresh Token Is Invalid");

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Error.InvalidCredentials("User.InvalidCrendentials");

            await _refreshTokenService.RevokeRefreshTokenAsync(requestDTO.RefreshToken);

            var newAccessToken = await _jwtService.GenerateTokenAsync(user);
            var newRefreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user.Id);

            return new TokenResponseDTO
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task<Result> LogoutAsync(RefreshRequestDTO requestDTO)
        {
            await _refreshTokenService.RevokeRefreshTokenAsync(requestDTO.RefreshToken);
            return Result.OK();
        }

        public async Task<Result> ChangePasswordAsync(string userEmail, ChangePasswordDTO passwordDTO)
        {
            if (passwordDTO.NewPassword != passwordDTO.ConfirmNewPassword)
                return Result.Fail(Error.InvalidCredentials("User.InvalidCrendentials", "New Password And Confirmation Password Do Not Mtch"));

            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user is null)
                return Result.Fail(Error.Unauthorized("User.Unauthorized"));

            var result = await _userManager.ChangePasswordAsync(user, passwordDTO.CurrentPassword, passwordDTO.NewPassword);
            if (!result.Succeeded)
                return Result.Fail(result.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList());

            return Result.OK();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is not null;
        }

        public async Task<Result> ForgotPasswordAsync(ForgotPasswordDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
            {
                // To prevent email enumeration, return success even if user not found.
                _logger.LogInformation("Forgot password requested for non-existent email {Email}", dto.Email);
                return Result.OK();
            }

            var otp = new Random().Next(100000, 999999).ToString();
            _logger.LogInformation("Generated OTP: {OTP} for {Email}", otp, dto.Email);

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };
            _memoryCache.Set($"OTP_{dto.Email}", otp, cacheOptions);

            string body = $$"""
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
</head>
<body style="margin: 0; padding: 0; background-color: #f6f4ee; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;">
    <div style="background-color: #f6f4ee; padding: 40px 20px; width: 100%; box-sizing: border-box;">
        
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px; background-color: #ffffff; border-radius: 16px; overflow: hidden; box-shadow: 0 4px 10px rgba(0,0,0,0.03);">
            
            <tr>
                <td style="padding: 30px 40px; text-align: center; border-bottom: 1px solid #f0f0f0;">
                    <span style="color: #cda25b; font-size: 20px; font-weight: 800; letter-spacing: 3px;">&mdash; NETFOODIA</span>
                </td>
            </tr>
            
            <tr>
                <td style="padding: 40px;">
                    <h2 style="margin: 0 0 20px 0; color: #1a1a1a; font-size: 26px;">Password Reset Request</h2>
                    <p style="margin: 0 0 25px 0; color: #6b7280; font-size: 16px; line-height: 1.6;">
                        Hello,<br><br>
                        We received a request to reset the password for your NetFoodia account. Please use the verification code below to proceed. This code is valid for <strong>10 minutes</strong>.
                    </p>
                    
                    <div style="text-align: center; margin: 35px 0;">
                        <span style="display: inline-block; background-color: #1a1a1a; color: #ffffff; font-size: 36px; font-weight: bold; padding: 15px 40px; border-radius: 12px; letter-spacing: 8px;">
                            {{otp}}
                        </span>
                    </div>
                    
                    <p style="margin: 0; color: #6b7280; font-size: 14px; line-height: 1.6;">
                        If you didn't request a password reset, you can safely ignore this email. Your account remains secure.
                    </p>
                </td>
            </tr>
            
            <tr>
                <td style="padding: 24px 40px; background-color: #faf9f6; text-align: center; border-top: 1px solid #f0f0f0;">
                    <p style="margin: 0; color: #9ca3af; font-size: 12px; line-height: 1.5;">
                        &copy; 2026 NetFoodia Workspace. All rights reserved.<br>
                        Deliver Impact.
                    </p>
                </td>
            </tr>
            
        </table>
    </div>
</body>
</html>
""";
            await _emailService.SendEmailAsync(dto.Email, "Password Reset OTP", body);

            return Result.OK();
        }

        public async Task<Result> ResetPasswordAsync(ResetPasswordDTO dto)
        {
            if (!_memoryCache.TryGetValue($"OTP_{dto.Email}", out string? storedOtp) || storedOtp != dto.OTP)
            {
                return Result.Fail(Error.Validation("Auth.InvalidOTP", "Invalid or expired OTP."));
            }

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
            {
                return Result.Fail(Error.Validation("Auth.UserNotFound", "User not found."));
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, dto.NewPassword);

            if (!result.Succeeded)
            {
                return Result.Fail(result.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList());
            }

            _memoryCache.Remove($"OTP_{dto.Email}");

            return Result.OK();
        }
    }
}
