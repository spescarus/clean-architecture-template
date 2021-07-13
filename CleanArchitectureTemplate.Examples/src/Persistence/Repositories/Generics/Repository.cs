using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SP.SampleCleanArchitectureTemplate.Application.RepositoryInterfaces.Generics;
using SP.SampleCleanArchitectureTemplate.Domain.Base.Interfaces;
using SP.SampleCleanArchitectureTemplate.Domain.Users;

namespace SP.SampleCleanArchitectureTemplate.Persistence.Repositories.Generics
{
    [ExcludeFromCodeCoverage]
    public abstract class Repository<TEntity, TEntityId>
        : QueryRepository<TEntity, TEntityId>,
          IRepository<TEntity, TEntityId>
        where TEntity : class, IBasicEntity<TEntityId>
        where TEntityId : struct
    {
        protected Repository([NotNull] DbContext context)
            : base(context)
        {
        }

        public async Task<TEntity> AddAsync(TEntity entity,
                                            UserId userId)
        {
            if (entity is IAuthoredAuditableEntity authoredAuditableEntity)
            {
                authoredAuditableEntity.CreatedBy = userId;
                authoredAuditableEntity.UpdatedBy = userId;
            }

            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = entity.CreatedAt;

            var result = await Entities.AddAsync(entity);
            return result.Entity;
        }

        public async Task<IReadOnlyCollection<TEntity>> AddAsync(IEnumerable<TEntity> entities,
                                                                 UserId userId)
        {
            var returnEntities = new List<TEntity>();
            foreach (var entity in entities)
            {
                var returnEntity = await AddAsync(entity, userId);
                returnEntities.Add(returnEntity);
            }

            return returnEntities;
        }

        public Task<TEntity> Update(TEntity entity,
                                    UserId userId)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            if (entity is IAuthoredAuditableEntity authoredAuditableEntity)
            {
                authoredAuditableEntity.UpdatedBy = userId;
            }

            return Task.FromResult(Entities.Update(entity)
                                           .Entity);
        }

        public virtual void Delete(TEntity entity,
                                   UserId userId,
                                   DeleteType deleteType)
        {
            if (deleteType == DeleteType.Soft)
            {
                entity.DeletedAt = DateTime.UtcNow;
                if (entity is IAuthoredSoftDeletableEntity softDeletableEntity)
                {
                    softDeletableEntity.DeletedBy = userId;
                }

                Entities.Update(entity);
            }
            else
            {
                Entities.Remove(entity);
            }
        }

        public async Task<bool> Exists(TEntityId id)
        {
            return await Entities.AnyAsync(e => e.Id.Equals(id));
        }

        protected abstract void OnDelete(TEntity entity,
                                         UserId userId,
                                         DeleteType deleteType);
    }
}
