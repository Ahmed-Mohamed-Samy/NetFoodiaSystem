using NetFoodia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Domain.Contracts
{
    public interface ISpecification<TEntity> where TEntity : BaseEntity
    {
        Expression<Func<TEntity,bool>>? Criteria { get; }
        ICollection<Expression<Func<TEntity, object>>> Includes { get; }
        Expression<Func<TEntity, object>>? OrderBy { get; }
        Expression<Func<TEntity, object>>? OrderByDesc { get; }
        int Take { get; }
        int Skip { get; }
        bool IsPaginated { get; }
    }
}
