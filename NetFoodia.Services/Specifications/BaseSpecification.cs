using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities;
using System.Linq.Expressions;

namespace NetFoodia.Services.Specifications
{
    public abstract class BaseSpecification<TEntity> : ISpecification<TEntity> where TEntity : BaseEntity
    {
        public Expression<Func<TEntity, bool>>? Criteria { get; }
        protected BaseSpecification(Expression<Func<TEntity, bool>>? criteria = null) => Criteria = criteria;

        #region Includes
        public ICollection<Expression<Func<TEntity, object>>> Includes { get; } = [];
        protected void AddInclude(Expression<Func<TEntity, object>> includeExp)
        {
            Includes.Add(includeExp);
        }
        #endregion

        #region Sorting
        public Expression<Func<TEntity, object>>? OrderBy { get; private set; }
        public Expression<Func<TEntity, object>>? OrderByDesc { get; private set; }
        protected void AddOrderBy(Expression<Func<TEntity, object>> orderBy) => OrderBy = orderBy;

        protected void AddOrderByDesc(Expression<Func<TEntity, object>> orderBy) => OrderByDesc = orderBy;
        #endregion

        #region Pagination

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPaginated { get; private set; }

        protected void ApplyPagination(int pageSize, int pageIndex)
        {
            IsPaginated = true;
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize;
        }

        #endregion
    }
}
