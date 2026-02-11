using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities;
using NetFoodia.Persistence.Data.DbContexts;

namespace NetFoodia.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NetFoodiaDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = [];
        public IRefreshTokenRepository RefreshTokenRepository { get; }

        public UnitOfWork(NetFoodiaDbContext dbContext, IRefreshTokenRepository refreshTokenRepository)
        {
            _dbContext = dbContext;
            RefreshTokenRepository = refreshTokenRepository;
        }


        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            var TypeEntity = typeof(TEntity);

            if (_repositories.TryGetValue(TypeEntity, out object? Repository))
                return (IGenericRepository<TEntity>)Repository;

            var newRepo = new GenericRepository<TEntity>(_dbContext);
            _repositories[TypeEntity] = newRepo;
            return newRepo;
        }


        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    }
}
