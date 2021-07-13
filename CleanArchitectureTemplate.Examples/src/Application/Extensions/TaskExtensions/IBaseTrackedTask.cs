using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SP.SampleCleanArchitectureTemplate.Application.Extensions.TaskExtensions
{
    public interface IBaseTrackedTask<TEntity>
    {
        Task<TEntity> AsNoTracking();
        TaskAwaiter<TEntity> GetAwaiter();
    }
}
