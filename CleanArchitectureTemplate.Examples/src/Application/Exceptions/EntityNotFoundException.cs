using System.Diagnostics.CodeAnalysis;

namespace SP.SampleCleanArchitectureTemplate.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class EntityNotFoundException<TEntity, TEntityId> : EntityNotFoundException
    {
        public EntityNotFoundException(TEntityId entityId)
            : base(typeof(TEntity).Name, entityId.ToString())
        {
        }
    }

    [ExcludeFromCodeCoverage]
    public abstract class EntityNotFoundException : DomainException
    {
        protected EntityNotFoundException(string entityName,
                                          string entityId)
            : base($"the {entityName} wasn't found with id {entityId}", DomainCodeErrors.EntityNotFound)
        {
            AddError("EntityType", entityName);
            AddError("Id",         entityId);
        }
    }
}
