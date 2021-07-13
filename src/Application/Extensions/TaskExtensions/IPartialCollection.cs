using System.Collections.Generic;

namespace SP.CleanArchitectureTemplate.Application.Extensions.TaskExtensions
{
    public interface IPartialCollection<TEntity>
    {
        IReadOnlyCollection<TEntity> Values { get; }
        long                         Count  { get; }
        int?                         Offset { get; }
        int?                         Limit  { get; }
    }
}
