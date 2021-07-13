using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SP.CleanArchitectureTemplate.Application.Extensions.TaskExtensions;
using SP.CleanArchitectureTemplate.Application.RepositoryInterfaces.Generics;
using SP.CleanArchitectureTemplate.Domain.Base.Interfaces;
using SP.CleanArchitectureTemplate.Persistence.TaskExtensions;

namespace SP.CleanArchitectureTemplate.Persistence.Repositories.Generics
{
    [ExcludeFromCodeCoverage]
    public abstract class QueryRepository<TEntity, TEntityId> : IQueryRepository<TEntity, TEntityId>
        where TEntity : class, IBasicEntity<TEntityId>
        where TEntityId : struct
    {
        protected QueryRepository([NotNull] DbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected DbContext           Context     { get; }
        protected DbSet<TEntity>      Entities    => Context.Set<TEntity>();
        protected IQueryable<TEntity> EntityQuery => DefaultIncludes(Entities);

        public virtual IBaseTrackedTask<TEntity> GetByIdAsync(TEntityId entityId)
        {
            return EntityQuery.Where(q => q.Id.Equals(entityId))
                              .ToTask()
                              .FirstOrDefaultAsync();
        }

        public virtual ITrackedCollectionTask<TEntity> GetAllAsync(params Expression<Func<TEntity, object>>[] includes)
        {
            return EntityQuery
                  .Includes(includes)
                  .ToTask()
                  .ToListAsync();
        }

        public virtual ITrackedCollectionTask<TEntity> GetAllByAsync(Expression<Func<TEntity, bool>> predicate,
                                                                     params Expression<Func<TEntity, object>>[]
                                                                         includes)
        {
            return EntityQuery
                  .Includes(includes)
                  .Where(predicate)
                  .ToTask()
                  .ToListAsync();
        }

        public virtual IBaseTrackedTask<TEntity> GetFirstByAsync(Expression<Func<TEntity, bool>> predicate,
                                                                 params Expression<Func<TEntity, object>>[] includes)
        {
            return EntityQuery
                  .Includes(includes)
                  .Where(predicate)
                  .ToTask()
                  .FirstOrDefaultAsync();
        }

        protected virtual IQueryable<TEntity> DefaultIncludes(IQueryable<TEntity> queryable)
        {
            return queryable;
        }
    }
}
