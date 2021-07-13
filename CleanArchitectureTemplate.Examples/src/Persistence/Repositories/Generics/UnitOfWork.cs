using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using SP.SampleCleanArchitectureTemplate.Application.RepositoryInterfaces.Generics;
using SP.SampleCleanArchitectureTemplate.Persistence.Context;

namespace SP.SampleCleanArchitectureTemplate.Persistence.Repositories.Generics
{
    [ExcludeFromCodeCoverage]
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext                       _context;
        private readonly IEnumerable<IDbContextInterceptor> _interceptors;

        public UnitOfWork(AppDbContext                       context,
                          IEnumerable<IDbContextInterceptor> interceptors)
        {
            _context      = context;
            _interceptors = interceptors;
        }

        public async Task<IScopedUnitOfWork> CreateScopeAsync()
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            return new ScopedUnitOfWork(_context, _interceptors, transaction);
        }

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            foreach (var interceptor in _interceptors)
            {
                interceptor.Intercept();
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
