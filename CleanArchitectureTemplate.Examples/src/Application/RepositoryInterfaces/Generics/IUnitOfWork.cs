using System.Threading;
using System.Threading.Tasks;

namespace SP.SampleCleanArchitectureTemplate.Application.RepositoryInterfaces.Generics
{
    public interface IUnitOfWork
    {
        Task SaveAsync(CancellationToken cancellationToken = default);
        Task<IScopedUnitOfWork> CreateScopeAsync();
    }
}
