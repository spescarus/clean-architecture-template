namespace SP.CleanArchitectureTemplate.Domain.Base.Interfaces
{
    public interface IEntity
        : IBasicEntity,
          IAuthoredAuditableEntity,
          IAuthoredSoftDeletableEntity
    {
    }

    public interface IEntity<TId> : IBasicEntity<TId>, IEntity
        where TId : struct
    {
    }
}
