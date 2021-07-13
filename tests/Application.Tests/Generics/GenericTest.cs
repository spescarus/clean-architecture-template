using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using SP.CleanArchitectureTemplate.Application.RepositoryInterfaces.Generics;
using SP.CleanArchitectureTemplate.Domain.Base.Interfaces;
using SP.CleanArchitectureTemplate.Domain.Users;
using SP.CleanArchitectureTemplate.Persistence.TaskExtensions;

namespace SP.CleanArchitectureTemplate.Application.Tests.Generics
{
    /// <summary>
    ///     Generic test class containing useful mock methods
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public abstract class GenericTest<TService>
    {
        protected GenericTest()
        {
            UnitOfWorkMock       = new Mock<IUnitOfWork>();
            LoggerMock           = new Mock<ILogger<TService>>();
            ExecutionContextMock = new Mock<IExecutionContext>();

            MockUnitOfWork();
            MockExecutionContext();
        }

        protected Mock<IUnitOfWork>       UnitOfWorkMock       { get; set; }
        protected Mock<ILogger<TService>> LoggerMock           { get; set; }
        protected Mock<IExecutionContext> ExecutionContextMock { get; set; }

        protected void MockUnitOfWork()
        {
            var mockScope = new Mock<IScopedUnitOfWork>();
            mockScope.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
                     .Returns((CancellationToken token) => Task.Run(() => { }, token));
            mockScope.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>()))
                     .Returns((CancellationToken token) => Task.Run(() => { }, token));

            UnitOfWorkMock.Setup(x => x.CreateScopeAsync())
                          .ReturnsAsync(mockScope.Object);

            UnitOfWorkMock.Setup(x => x.SaveAsync(It.IsAny<CancellationToken>()))
                          .Returns((CancellationToken token) => Task.Run(() => { }, token));
        }

        protected void MockExecutionContext()
        {
            ExecutionContextMock?.Setup(x => x.UserIdCaller)
                                 .Returns(new UserId());
        }

        /// <summary>
        ///     Mock for AddAsync method
        /// </summary>
        /// <remarks>
        ///     Entities passed as parameter to AddAsync will be added to the collection passed in parameter to this method
        /// </remarks>
        /// <typeparam name="TRepo">Repository to mock</typeparam>
        /// <typeparam name="T">Entity of the repository</typeparam>
        /// <typeparam name="TId">Strongly typed id of the entity</typeparam>
        /// <param name="mockRepo">Repository to mock</param>
        /// <param name="entities">List of entities the method will add into</param>
        protected void MockAddAsync<TRepo, T, TId>(Mock<TRepo>    mockRepo,
                                                   ICollection<T> entities)
            where TRepo : class, IRepository<T, TId>
            where T : class, IBasicEntity<TId>
            where TId : struct
        {
            mockRepo.Setup(x => x.AddAsync(It.IsAny<T>(), It.IsAny<UserId>()))
                    .Returns((T      entity,
                              UserId userId) => Task.Run(() =>
                     {
                         entities.Add(entity);
                         return entity;
                     }));
            mockRepo.Setup(x => x.AddAsync(It.IsAny<IEnumerable<T>>(), It.IsAny<UserId>()))
                    .Returns((IEnumerable<T> entitiesToAdd,
                              UserId         userId) => Task.Run<IReadOnlyCollection<T>>(() =>
                     {
                         var returnEntities = new List<T>();
                         foreach (var entity in entitiesToAdd)
                         {
                             returnEntities.Add(entity);
                         }

                         return returnEntities;
                     }));
        }

        /// <summary>
        ///     Mock for Update method
        /// </summary>
        /// <remarks>
        ///     Entities passed as parameter to Update will be searched and updated in the collection passed in parameter to this
        ///     method
        /// </remarks>
        /// <typeparam name="TRepo">Repository to mock</typeparam>
        /// <typeparam name="T">Entity of the repository</typeparam>
        /// <typeparam name="TId">Strongly typed id of the entity</typeparam>
        /// <param name="mockRepo">Repository to mock</param>
        /// <param name="entities">List of entities the method will look for and update</param>
        protected void MockUpdate<TRepo, T, TId>(Mock<TRepo>    mockRepo,
                                                 ICollection<T> entities)
            where TRepo : class, IRepository<T, TId>
            where T : class, IBasicEntity<TId>
            where TId : struct
        {
            mockRepo.Setup(x => x.Update(It.IsAny<T>(), It.IsAny<UserId>()))
                    .Returns((T      entity,
                              UserId userId) => Task.Run(() =>
                     {
                         var e = entities.FirstOrDefault(x => x.Id.Equals(entity.Id));
                         if (e == null)
                         {
                             return null;
                         }

                         e = entity;
                         return e;
                     }));
        }

        /// <summary>
        ///     Mock for Delete method
        /// </summary>
        /// <remarks>
        ///     Entities passed as parameter to Delete will be searched and removed from the collection passed in parameter to this
        ///     method
        /// </remarks>
        /// <typeparam name="TRepo">Repository to mock</typeparam>
        /// <typeparam name="T">Entity of the repository</typeparam>
        /// <typeparam name="TId">Strongly typed id of the entity</typeparam>
        /// <param name="mockRepo">Repository to mock</param>
        /// <param name="entities">List of entities the method will look for and delete</param>
        protected void MockDelete<TRepo, T, TId>(Mock<TRepo>    mockRepo,
                                                 ICollection<T> entities)
            where TRepo : class, IRepository<T, TId>
            where T : class, IBasicEntity<TId>
            where TId : struct
        {
            mockRepo.Setup(x => x.Delete(It.IsAny<T>(), It.IsAny<UserId>(), It.IsAny<DeleteType>()))
                    .Callback((T          entity,
                               UserId     userId,
                               DeleteType type) => Task.Run(() =>
                     {
                         var e = entities.FirstOrDefault(x => x.Id.Equals(entity.Id));
                         if (e == null)
                         {
                             return;
                         }

                         if (type == DeleteType.Soft)
                         {
                             e.DeletedAt = DateTime.UtcNow;
                         }
                         else
                         {
                             entities.Remove(e);
                         }
                     }));
        }

        /// <summary>
        ///     Mock for GetByIdAsync method
        /// </summary>
        /// <remarks>
        ///     Collection passed in parameter to this method will be used as source
        /// </remarks>
        /// <typeparam name="TRepo">Repository to mock</typeparam>
        /// <typeparam name="T">Entity of the repository</typeparam>
        /// <typeparam name="TId">Strongly typed id of the entity</typeparam>
        /// <param name="mockRepo">Repository to mock</param>
        /// <param name="entities">List of entities the method will use as source</param>
        protected void MockGetByIdAsync<TRepo, T, TId>(Mock<TRepo>    mockRepo,
                                                       ICollection<T> entities)
            where TRepo : class, IRepository<T, TId>
            where T : class, IBasicEntity<TId>
            where TId : struct
        {
            mockRepo.Setup(x => x.GetByIdAsync(It.IsAny<TId>()))
                    .Returns((TId id) =>
                     {
                         var asyncEntities = entities.AsQueryable()
                                                     .BuildMock();
                         var e = asyncEntities.Object
                                              .Where(x => x.Id.Equals(id))
                                              .ToTask()
                                              .FirstOrDefaultAsync();
                         return e;
                     });
        }

        /// <summary>
        ///     Mock for GetAllAsync method
        /// </summary>
        /// <remarks>
        ///     Collection passed in parameter to this method will be used as source
        /// </remarks>
        /// <typeparam name="TRepo">Repository to mock</typeparam>
        /// <typeparam name="T">Entity of the repository</typeparam>
        /// <typeparam name="TId">Strongly typed id of the entity</typeparam>
        /// <param name="mockRepo">Repository to mock</param>
        /// <param name="entities">List of entities the method will use as source</param>
        protected void MockGetAllAsync<TRepo, T, TId>(Mock<TRepo>    mockRepo,
                                                      ICollection<T> entities)
            where TRepo : class, IRepository<T, TId>
            where T : class, IBasicEntity<TId>
            where TId : struct
        {
            mockRepo.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<T, object>>[]>()))
                    .Returns((Expression<Func<T, object>>[] includes) =>
                     {
                         var asyncEntities = entities.AsQueryable()
                                                     .BuildMock();
                         return asyncEntities.Object
                                             .ToTask()
                                             .ToListAsync();
                     });
        }

        /// <summary>
        ///     Mock for GetAllByAsync method
        /// </summary>
        /// <remarks>
        ///     Collection passed in parameter to this method will be used as source
        /// </remarks>
        /// <typeparam name="TRepo">Repository to mock</typeparam>
        /// <typeparam name="T">Entity of the repository</typeparam>
        /// <typeparam name="TId">Strongly typed id of the entity</typeparam>
        /// <param name="mockRepo">Repository to mock</param>
        /// <param name="entities">List of entities the method will use as source</param>
        protected void MockGetAllByAsync<TRepo, T, TId>(Mock<TRepo>    mockRepo,
                                                        ICollection<T> entities)
            where TRepo : class, IRepository<T, TId>
            where T : class, IBasicEntity<TId>
            where TId : struct
        {
            mockRepo.Setup(x => x.GetAllByAsync(It.IsAny<Expression<Func<T, bool>>>(), It.IsAny<Expression<Func<T, object>>[]>()))
                    .Returns((Expression<Func<T, bool>>     exp,
                              Expression<Func<T, object>>[] includes) =>
                     {
                         var asyncEntities = entities.AsQueryable()
                                                     .BuildMock();
                         return asyncEntities.Object
                                             .Where(exp)
                                             .ToTask()
                                             .ToListAsync();
                     });
        }

        /// <summary>
        ///     Mock for GetFirstByAsync method
        /// </summary>
        /// <remarks>
        ///     Collection passed in parameter to this method will be used as source
        /// </remarks>
        /// <typeparam name="TRepo">Repository to mock</typeparam>
        /// <typeparam name="T">Entity of the repository</typeparam>
        /// <typeparam name="TId">Strongly typed id of the entity</typeparam>
        /// <param name="mockRepo">Repository to mock</param>
        /// <param name="entities">List of entities the method will use as source</param>
        protected void MockGetFirstByAsync<TRepo, T, TId>(Mock<TRepo>    mockRepo,
                                                          ICollection<T> entities)
            where TRepo : class, IRepository<T, TId>
            where T : class, IBasicEntity<TId>
            where TId : struct
        {
            mockRepo.Setup(x => x.GetFirstByAsync(It.IsAny<Expression<Func<T, bool>>>(), It.IsAny<Expression<Func<T, object>>[]>()))
                    .Returns((Expression<Func<T, bool>>     exp,
                              Expression<Func<T, object>>[] includes) =>
                     {
                         var asyncEntities = entities.AsQueryable()
                                                     .BuildMock();
                         return asyncEntities.Object
                                             .Where(exp)
                                             .ToTask()
                                             .FirstOrDefaultAsync();
                     });
        }
    }
}
