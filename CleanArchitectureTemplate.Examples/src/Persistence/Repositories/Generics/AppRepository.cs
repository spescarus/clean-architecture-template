using System.Diagnostics.CodeAnalysis;
using SP.SampleCleanArchitectureTemplate.Domain.Base.Interfaces;
using SP.SampleCleanArchitectureTemplate.Persistence.Context;

namespace SP.SampleCleanArchitectureTemplate.Persistence.Repositories.Generics
{
    [ExcludeFromCodeCoverage]
    public abstract class AppRepository<TEntity, TEntityId> : Repository<TEntity, TEntityId>
        where TEntity : class, IBasicEntity<TEntityId>
        where TEntityId : struct
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        protected AppRepository(AppDbContext context)
            : base(context)
        {
        }
    }
}
