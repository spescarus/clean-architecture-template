using System.Collections.Generic;

namespace SP.SampleCleanArchitectureTemplate.Application.Extensions.TaskExtensions
{
    public interface IPartialCollection<out TEntity>
    {
        IReadOnlyCollection<TEntity> Values { get; }
        long                         Count  { get; }
        int?                         Offset { get; }
        int?                         Limit  { get; }
    }
}
