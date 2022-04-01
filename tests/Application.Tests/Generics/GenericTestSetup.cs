using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using MockQueryable.Moq;
using Moq;
using SP.CleanArchitectureTemplate.Application.RepositoryInterfaces.Generics;
using SP.CleanArchitectureTemplate.Domain.Base.Interfaces;
using SP.CleanArchitectureTemplate.Persistence.TaskExtensions;

namespace SP.CleanArchitectureTemplate.Application.Tests.Generics
{
    [ExcludeFromCodeCoverage]
    public class GenericTestSetup
    {
        protected static void MockGetByIdAsync<TRepo, T, TId>(Mock<TRepo>    mockRepo,
                                                              ICollection<T> entities)
            where TRepo : class, IRepository<T, TId>
            where T : class, IBasicEntity<TId>
            where TId : struct
        {
            mockRepo.Setup(x => x.GetByIdAsync(It.IsAny<TId>()))
                    .Returns((TId id) =>
                     {
                         Debugger.Break();
                         var asyncEntities = entities.AsQueryable()
                                                     .BuildMock();
                         var test = asyncEntities
                                             .Where(x => x.Id.Equals(id))
                                             .ToTask()
                                             .FirstOrDefaultAsync();

                         return test;
                     });
        }

        protected static void MockGetAllByAsync<TRepo, T, TId>(Mock<TRepo>    mockRepo,
                                                               ICollection<T> entities)
            where TRepo : class, IRepository<T, TId>
            where T : class, IBasicEntity<TId>
            where TId : struct => mockRepo.Setup(x => x.GetAllByAsync(It.IsAny<Expression<Func<T, bool>>>(), It.IsAny<Expression<Func<T, object>>[]>()))
                                          .Returns((Expression<Func<T, bool>>     exp,
                                                    Expression<Func<T, object>>[] includes) =>
                                           {
                                               var asyncEntities = entities.AsQueryable()
                                                                           .BuildMock();
                                               return asyncEntities
                                                                   .Where(exp)
                                                                   .ToTask()
                                                                   .ToListAsync();
                                           });

        protected static void MockGetFirstByAsync<TRepo, T, TId>(Mock<TRepo>    mockRepo,
                                                                 ICollection<T> entities)
            where TRepo : class, IRepository<T, TId>
            where T : class, IBasicEntity<TId>
            where TId : struct => mockRepo.Setup(x => x.GetFirstByAsync(It.IsAny<Expression<Func<T, bool>>>(), It.IsAny<Expression<Func<T, object>>[]>()))
                                          .Returns((Expression<Func<T, bool>>     exp,
                                                    Expression<Func<T, object>>[] includes) =>
                                           {
                                               var asyncEntities = entities.AsQueryable()
                                                                           .BuildMock();
                                               return asyncEntities
                                                                   .Where(exp)
                                                                   .ToTask()
                                                                   .FirstOrDefaultAsync();
                                           });
    }
}
