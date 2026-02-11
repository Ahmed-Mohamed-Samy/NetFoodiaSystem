using Microsoft.EntityFrameworkCore;
using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities.IdentityModule;
using NetFoodia.Persistence.Data.DbContexts;
using System.Linq.Expressions;

namespace NetFoodia.Persistence.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly NetFoodiaDbContext _dbContext;

        public RefreshTokenRepository(NetFoodiaDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(RefreshToken refreshToken) => await _dbContext.RefreshTokens.AddAsync(refreshToken);


        public async Task<RefreshToken?> FirstOrDefaultAsync(Expression<Func<RefreshToken, bool>> predicate)
        {
            return await _dbContext.RefreshTokens.FirstOrDefaultAsync(predicate);
        }

    }
}
