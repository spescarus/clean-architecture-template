using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SP.SampleCleanArchitectureTemplate.Application.Extensions.TaskExtensions;

namespace SP.SampleCleanArchitectureTemplate.Persistence.TaskExtensions
{
    [ExcludeFromCodeCoverage]
    public class PartialCollection<TEntity> : IPartialCollection<TEntity>
    {
        public PartialCollection(IReadOnlyCollection<TEntity> values,
                                 long                         count,
                                 int                          offset,
                                 int                          limit)
        {
            Values = values;
            Count  = count;
            Offset = offset;
            Limit  = limit;
        }

        public PartialCollection(IReadOnlyCollection<TEntity> values,
                                 long                         count)
        {
            Values = values;
            Count  = count;
            Offset = null;
            Limit  = null;
        }

        public IReadOnlyCollection<TEntity> Values { get; }
        public long                         Count  { get; }
        public int?                         Offset { get; }
        public int?                         Limit  { get; }
    }
}
