namespace SP.SampleCleanArchitectureTemplate.Application.Extensions.TaskExtensions
{
    public interface IPaginatedTrackedCollectionTask<TEntity> : IBaseTrackedTask<IPartialCollection<TEntity>>
    {
    }
}
