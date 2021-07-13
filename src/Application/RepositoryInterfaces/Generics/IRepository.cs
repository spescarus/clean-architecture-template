using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using SP.CleanArchitectureTemplate.Domain.Base.Interfaces;
using SP.CleanArchitectureTemplate.Domain.Users;

namespace SP.CleanArchitectureTemplate.Application.RepositoryInterfaces.Generics
{
    public interface IRepository<TEntity, TEntityId> : IQueryRepository<TEntity, TEntityId>
        where TEntity : class, IBasicEntity<TEntityId>
        where TEntityId : struct
    {
        Task<TEntity> AddAsync([NotNull] TEntity entity,
                               UserId            userId);

        Task<IReadOnlyCollection<TEntity>> AddAsync([NotNull] IEnumerable<TEntity> entities,
                                                    UserId                         userId);

        Task<TEntity> Update([NotNull] TEntity entity,
                             UserId            userId);

        void Delete([NotNull] TEntity entity,
                    UserId            userId,
                    DeleteType        deleteType = DeleteType.Soft);

        Task<bool> Exists(TEntityId id);
    }
}
