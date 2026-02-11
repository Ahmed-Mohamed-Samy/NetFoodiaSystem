using NetFoodia.Domain.Entities;
using System.Linq.Expressions;

namespace NetFoodia.Domain.Contracts
{
    public interface ISpecification<TEntity> where TEntity : BaseEntity
    {
        Expression<Func<TEntity, bool>>? Criteria { get; }
        ICollection<Expression<Func<TEntity, object>>> Includes { get; }
        Expression<Func<TEntity, object>>? OrderBy { get; }
        Expression<Func<TEntity, object>>? OrderByDesc { get; }
        int Take { get; }
        int Skip { get; }
        bool IsPaginated { get; }
    }
}
