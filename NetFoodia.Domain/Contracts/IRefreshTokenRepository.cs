using NetFoodia.Domain.Entities.IdentityModule;
using System.Linq.Expressions;

namespace NetFoodia.Domain.Contracts
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken);
        Task<RefreshToken?> FirstOrDefaultAsync(Expression<Func<RefreshToken, bool>> predicate);
    }
}
