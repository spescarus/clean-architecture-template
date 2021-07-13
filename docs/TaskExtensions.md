# Task Extensions

## What is it ?
This is a series of extensions on queryable used for delegating the pagination and the tracking responsibility into the service.

With this kind of methods, any query written can be paginated if it's a collection, or can have its tracking disabled without changing anything in the repository.

The thing to have in mind is if you paginate with this extension, the pagination will be on sql server database and not on the client side.

## How can I use these extensions ?

The best example is the QueryRepository

```csharp
public virtual ITrackedCollectionTask<TEntity> GetAllAsync(params Expression<Func<TEntity, object>>[] includes)
        {
            return EntityQuery
                  .Includes(includes)
                  .ToTask()
                  .ToListAsync();
        }
```

In this method, there is a ToTask() extension. This extension allows you to choose if you want to create a collection query or a single statement.

As you can see, the method now returns *ITrackedCollectionTask\<TEntity>* and can be paginated in the service like the following example:

```csharp
public void TestTaskExtensions() {
    var entitiesTrackedWithoutPaginate = await _repository.GetAllAsync();
    var entityTrackedWithPagination = await _repository.GetAllAsync()
                                                       .Paginate(20, 10);
    var entityWithoutTrackingAndPagination = await _repository.GetAllAsync()
                                                              .AsNoTracking();
    var entityWithoutTrackingAndWithPagination = await _repository.GetAllAsync()
                                                                  .Paginate(20, 10)
                                                                  .AsNoTracking();
}
```

