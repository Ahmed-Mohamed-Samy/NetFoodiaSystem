using NetFoodia.Domain.Entities;

namespace NetFoodia.Domain.Contracts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
        IRefreshTokenRepository RefreshTokenRepository { get; }

    }
}
