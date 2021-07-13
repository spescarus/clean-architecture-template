namespace SP.CleanArchitectureTemplate.Domain.Base.Interfaces
{
    public interface IBasicEntity<TId> : IBasicEntity
        where TId : struct
    {
        TId Id { get; set; }
    }

    public interface IBasicEntity
        : IAuditableEntity,
          ISoftDeletableEntity
    {
    }
}
