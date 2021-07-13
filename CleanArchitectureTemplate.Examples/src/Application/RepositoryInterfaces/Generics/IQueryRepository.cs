using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using SP.SampleCleanArchitectureTemplate.Application.Extensions.TaskExtensions;
using SP.SampleCleanArchitectureTemplate.Domain.Base.Interfaces;

namespace SP.SampleCleanArchitectureTemplate.Application.RepositoryInterfaces.Generics
{
    public interface IQueryRepository<TEntity, TEntityId>
        where TEntity : class, IBasicEntity<TEntityId>
        where TEntityId : struct
    {
        IBaseTrackedTask<TEntity> GetByIdAsync(TEntityId entityId);

        ITrackedCollectionTask<TEntity> GetAllAsync(params Expression<Func<TEntity, object>>[] includes);

        ITrackedCollectionTask<TEntity> GetAllByAsync([NotNull] Expression<Func<TEntity, bool>>     predicate,
                                                      params    Expression<Func<TEntity, object>>[] includes);

        IBaseTrackedTask<TEntity> GetFirstByAsync([NotNull] Expression<Func<TEntity, bool>>     predicate,
                                                  params    Expression<Func<TEntity, object>>[] includes);
    }
}
