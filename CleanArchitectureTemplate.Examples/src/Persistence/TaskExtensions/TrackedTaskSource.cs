using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SP.SampleCleanArchitectureTemplate.Application.Extensions.TaskExtensions;

namespace SP.SampleCleanArchitectureTemplate.Persistence.TaskExtensions
{
    [ExcludeFromCodeCoverage]
    public class TrackedTaskSource<TEntity>
        : ITrackedTaskSource<TEntity>
        where TEntity : class
    {
        private readonly IQueryable<TEntity> _query;

        public TrackedTaskSource(IQueryable<TEntity> query)
        {
            _query = query;
        }

        public ITrackedTask<TEntity> FirstOrDefaultAsync()
        {
            return new TrackedTask<TEntity>(_query);
        }

        public ITrackedCollectionTask<TEntity> ToListAsync()
        {
            return new TrackedCollectionTask<TEntity>(_query);
        }
    }
}