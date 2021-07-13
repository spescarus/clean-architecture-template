namespace SP.SampleCleanArchitectureTemplate.Domain.Base.Interfaces
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
