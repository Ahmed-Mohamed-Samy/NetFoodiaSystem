using NetFoodia.Domain.Entities;

namespace NetFoodia.Domain.Contracts
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specifications);
        Task<TEntity?> GetByIdAsync(int id);
        Task<TEntity?> GetByIdAsync(ISpecification<TEntity> specifications);
        Task AddAsync(TEntity entity);
        void Remove(TEntity entity);
        void Update(TEntity entity);
        Task<int> CountAsync(ISpecification<TEntity> specifications);
    }
}
