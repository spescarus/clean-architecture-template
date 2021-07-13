namespace SP.CleanArchitectureTemplate.Application.Extensions.TaskExtensions
{
    public interface IPaginatedTrackedCollectionTask<TEntity> : IBaseTrackedTask<IPartialCollection<TEntity>>
    {
    }
}
