using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SP.CleanArchitectureTemplate.Application.RepositoryInterfaces.Generics;

namespace SP.CleanArchitectureTemplate.Persistence.Repositories.Generics.Interceptors
{
    [ExcludeFromCodeCoverage]
    public abstract class DbContextInterceptor : IDbContextInterceptor
    {
        private readonly DbContext _context;

        protected DbContextInterceptor(DbContext context)
        {
            _context = context;
        }

        public abstract void Intercept();

        protected IEnumerable<EntityEntry> Entries()
        {
            return _context.ChangeTracker.Entries();
        }

        protected IEnumerable<EntityEntry<T>> Entries<T>()
            where T : class
        {
            return _context.ChangeTracker.Entries<T>();
        }
    }
}
