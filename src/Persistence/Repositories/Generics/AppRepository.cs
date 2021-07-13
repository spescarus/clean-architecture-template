using System.Diagnostics.CodeAnalysis;
using SP.CleanArchitectureTemplate.Domain.Base.Interfaces;
using SP.CleanArchitectureTemplate.Persistence.Context;

namespace SP.CleanArchitectureTemplate.Persistence.Repositories.Generics
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
