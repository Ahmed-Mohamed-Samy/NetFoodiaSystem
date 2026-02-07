using Microsoft.EntityFrameworkCore;
using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NetFoodia.Persistence
{
    internal static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity>(IQueryable<TEntity> query, ISpecification<TEntity> spec) where TEntity : BaseEntity
        {
            if (spec is null) return query;

            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            if (spec.Includes is not null && spec.Includes.Any())
                query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            if (spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);
            else if (spec.OrderByDesc is not null)
                query = query.OrderByDescending(spec.OrderByDesc);


            if (spec.IsPaginated)
                query = query.Skip(spec.Skip).Take(spec.Take);



            return query;
        }
    }
}
