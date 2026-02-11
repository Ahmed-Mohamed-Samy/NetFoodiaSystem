using Microsoft.EntityFrameworkCore;
using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities;
using NetFoodia.Persistence.Data.DbContexts;

namespace NetFoodia.Persistence.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly NetFoodiaDbContext _dbContext;

        public GenericRepository(NetFoodiaDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(TEntity entity) => await _dbContext.Set<TEntity>().AddAsync(entity);


        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbContext.Set<TEntity>().ToListAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specifications)
        {
            var Query = SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specifications);

            return await Query.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id) => await _dbContext.Set<TEntity>().FindAsync(id);

        public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity> specifications)
        {
            return await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specifications).FirstOrDefaultAsync();
        }

        public void Remove(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);
        public void Update(TEntity entity) => _dbContext.Set<TEntity>().Update(entity);

        public async Task<int> CountAsync(ISpecification<TEntity> specifications)
        {
            return await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specifications).CountAsync();
        }
    }
}
