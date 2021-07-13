using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace SP.SampleCleanArchitectureTemplate.Persistence
{
    [ExcludeFromCodeCoverage]
    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> Includes<TEntity>(this   IQueryable<TEntity>                 dbset,
                                                            params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class
        {
            var query = dbset;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }
    }
}
