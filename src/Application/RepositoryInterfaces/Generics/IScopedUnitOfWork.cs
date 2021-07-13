﻿using System.Threading;
using System.Threading.Tasks;

namespace SP.CleanArchitectureTemplate.Application.RepositoryInterfaces.Generics
{
    public interface IScopedUnitOfWork
    {
        Task SaveAsync(CancellationToken   cancellationToken = default);
        Task CommitAsync(CancellationToken cancellationToken = default);
    }
}
