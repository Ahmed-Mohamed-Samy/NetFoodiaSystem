using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities.IdentityModule;
using NetFoodia.Services_Abstraction;
using System.Security.Cryptography;

namespace NetFoodia.Services.Security
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> GenerateRefreshTokenAsync(string userId)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = token,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            };

            var refreshRepo = _unitOfWork.RefreshTokenRepository;
            await refreshRepo.AddAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return token;
        }

        public async Task<string?> GetUserIdFromValidRefreshTokenAsync(string token)
        {
            var refreshRepo = _unitOfWork.RefreshTokenRepository;
            var refreshToken = await refreshRepo
                .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsRevoked && rt.ExpiryDate > DateTime.UtcNow);

            return refreshToken?.UserId;
        }

        public async Task RevokeRefreshTokenAsync(string token)
        {
            var refreshRepo = _unitOfWork.RefreshTokenRepository;
            var refreshToken = await refreshRepo
                .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsRevoked);

            if (refreshToken is null) return;


            refreshToken.IsRevoked = true;
            await _unitOfWork.SaveChangesAsync();

        }
    }
}
