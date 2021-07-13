using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SP.SampleCleanArchitectureTemplate.Domain.Base.Interfaces;
using SP.SampleCleanArchitectureTemplate.Domain.Users;
using SP.SampleCleanArchitectureTemplate.Persistence.Context;

namespace SP.SampleCleanArchitectureTemplate.Persistence.Repositories.Generics.Interceptors
{
    [ExcludeFromCodeCoverage]
    public class EntityInterceptor : DbContextInterceptor
    {
        public EntityInterceptor([NotNull] AppDbContext context)
            : base(context)
        {
        }

        public override void Intercept()
        {
            var entityEntries = Entries()
                               .Where(x => x.State != EntityState.Detached)
                               .ToArray();
            var contextUserId = FindContextAuthor(entityEntries);

            foreach (var entityEntry in entityEntries)
            {
                switch (entityEntry.State)
                {
                    case EntityState.Deleted:
                        HardDeleteEntity(entityEntry);
                        break;
                    case EntityState.Modified:
                        if (entityEntry.Entity is ISoftDeletableEntity softDeletableEntity &&
                            softDeletableEntity.DeletedAt.HasValue)
                        {
                            SoftDeleteEntity(entityEntry, contextUserId);
                        }

                        break;
                    case EntityState.Added:
                        AddedEntity(entityEntry.Entity);
                        break;
                }
            }
        }

        private void HardDeleteEntity([NotNull] EntityEntry entityEntry)
        {
            var cascadeEntities = entityEntry.Navigations.Where(p => p.IsLoaded)
                                             .ToArray();
            foreach (var navigationEntry in cascadeEntities)
            {
                switch (navigationEntry)
                {
                    case ReferenceEntry referenceEntry when referenceEntry.TargetEntry.State != EntityState.Deleted:
                        referenceEntry.TargetEntry.State = EntityState.Deleted;
                        HardDeleteEntity(referenceEntry.TargetEntry);
                        break;
                    case CollectionEntry collectionEntries:
                    {
                        HardDeleteCollectionEntries(collectionEntries);
                        break;
                    }
                }
            }
        }

        private void HardDeleteCollectionEntries(CollectionEntry collectionEntries)
        {
            foreach (var entity in collectionEntries.CurrentValue)
            {
                var subEntityEntry = Entries()
                   .FirstOrDefault(p => p.Entity == entity);
                if (subEntityEntry == null)
                {
                    continue;
                }

                if (subEntityEntry.State == EntityState.Deleted)
                {
                    continue;
                }

                subEntityEntry.State = EntityState.Deleted;
                HardDeleteEntity(subEntityEntry);
            }
        }

        private void SoftDeleteEntity([NotNull] EntityEntry entityEntry,
                                      UserId? currentUserId)
        {
            var cascadeEntities = entityEntry.Navigations.Where(p => p.IsLoaded)
                                             .ToArray();
            foreach (var navigationEntry in cascadeEntities)
            {
                switch (navigationEntry)
                {
                    case ReferenceEntry referenceEntry:
                    {
                        if (!(navigationEntry.CurrentValue is ISoftDeletableEntity navigationEntrySoftDeletableEntity) ||
                            navigationEntrySoftDeletableEntity.DeletedAt.HasValue)
                        {
                            continue;
                        }

                        ApplySoftDeleteEntity(referenceEntry.TargetEntry, navigationEntrySoftDeletableEntity, currentUserId);
                        SoftDeleteEntity(referenceEntry.TargetEntry, currentUserId);
                        break;
                    }
                    case CollectionEntry collectionEntries:
                    {
                        SoftDeleteCollectionEntries(currentUserId, collectionEntries);
                        break;
                    }
                }
            }
        }

        private void SoftDeleteCollectionEntries(UserId?         currentUserId,
                                             CollectionEntry collectionEntries)
        {
            foreach (var entity in collectionEntries.CurrentValue)
            {
                switch (entity)
                {
                    case ISoftDeletableEntity navigationEntrySoftDeletableEntity when !navigationEntrySoftDeletableEntity.DeletedAt.HasValue:
                    {
                        var subEntityEntry = Entries()
                           .FirstOrDefault(p => p.Entity == entity);
                        if (subEntityEntry == null)
                        {
                            continue;
                        }

                        ApplySoftDeleteEntity(subEntityEntry, navigationEntrySoftDeletableEntity, currentUserId);
                        SoftDeleteEntity(subEntityEntry, currentUserId);
                        break;
                    }
                }
            }
        }

        private static void ApplySoftDeleteEntity([NotNull] EntityEntry entityEntry,
                                                  [NotNull] ISoftDeletableEntity softDeletableEntity,
                                                  UserId? currentUserId)
        {
            softDeletableEntity.DeletedAt = DateTime.UtcNow;
            if (softDeletableEntity is IAuthoredSoftDeletableEntity authoredSoftDeletableEntity)
            {
                authoredSoftDeletableEntity.DeletedBy = currentUserId;
            }

            entityEntry.State = EntityState.Modified;
        }

        private static void AddedEntity([NotNull] object entity)
        {
            if (!(entity is IAuditableEntity auditableEntity))
            {
                return;
            }

            auditableEntity.UpdatedAt = DateTime.UtcNow;
            auditableEntity.CreatedAt = auditableEntity.UpdatedAt;
        }

        private static UserId? FindContextAuthor([NotNull] IReadOnlyCollection<EntityEntry> entityEntries)
        {
            var currentUserId = entityEntries.Where(p => p.State == EntityState.Added || p.State == EntityState.Modified)
                                             .Select(p => p.Entity)
                                             .OfType<IAuthoredAuditableEntity>()
                                             .FirstOrDefault()
                                            ?.UpdatedBy ??
                                entityEntries.Select(p => p.Entity)
                                             .OfType<IAuthoredSoftDeletableEntity>()
                                             .Where(p => p.DeletedBy.HasValue)
                                             .Select(p => p.DeletedBy)
                                             .FirstOrDefault();


            return currentUserId;
        }
    }
}
