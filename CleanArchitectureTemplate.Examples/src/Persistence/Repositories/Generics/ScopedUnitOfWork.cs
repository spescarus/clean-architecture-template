using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using SP.SampleCleanArchitectureTemplate.Application.RepositoryInterfaces.Generics;
using SP.SampleCleanArchitectureTemplate.Persistence.Context;

namespace SP.SampleCleanArchitectureTemplate.Persistence.Repositories.Generics
{
    [ExcludeFromCodeCoverage]
    public class ScopedUnitOfWork : IScopedUnitOfWork
    {
        private readonly AppDbContext                       _context;
        private readonly IEnumerable<IDbContextInterceptor> _interceptors;
        private readonly IDbContextTransaction              _transaction;

        public ScopedUnitOfWork(AppDbContext                       context,
                                IEnumerable<IDbContextInterceptor> interceptors,
                                IDbContextTransaction              transaction)
        {
            _context      = context;
            _interceptors = interceptors;
            _transaction  = transaction;
        }

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            foreach (var interceptor in _interceptors)
            {
                interceptor.Intercept();
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            foreach (var interceptor in _interceptors)
            {
                interceptor.Intercept();
            }

            try
            {
                await _transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await _transaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                _transaction?.Dispose();
            }
        }
    }
}
