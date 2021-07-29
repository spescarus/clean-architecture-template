<br />
<p align="center">

  <h3 align="center">Template Initialization</h3>
  <p align="center">
  In this document, you will find everything about the initialization of the template and how to configure it for Development environment.
  </p>
</p>

# Table of Contents

- [Table of Contents](#table-of-contents)
- [What does this template contain?](#what-does-this-template-contain)
  - [Domain](#domain)
    - [Typed Id](#typed-id)
  - [Infrastructure](#infrastructure)
  - [Persistence](#persistence)
    - [Configurations](#configurations)
    - [TaskExtensions](#taskextensions)
    - [Repositories](#repositories)
  - [Application](#services)
  - [Tests](#tests)
- [What do I need to do before I start coding?](#what-do-i-need-to-do-before-i-start-coding-)
  - [Change the solution name](#change-the-solution-name)
  - [Change the base namespace](#change-the-base-namespace)
  - [Fill the README](#fill-the-readme)
  - [Docker-compose](#docker-compose)
- [How to be sure my modification haven't blown the service up?](#how-to-be-sure-my-modification-havent-blown-the-service-up-)
- [I want to create a new class library!](#i-want-to-create-a-new-class-library-)

# What does this template contain?

## Domain

In this project you will find the basics to create the application entities.

Each entity can inherit from *BasicEntity* or *Entity*. All entities can be soft deleted and have a minimal audit information.

***BasicEntity*** are the entities with minimal auditing values like *Created At* or *Deleted At* and a [typed id](#typed-id).

***Entity*** are the entities with auditing and author for each audit information like *Created By* or *Deleted By*.

By default, these author fields are UserId which are defined in the same project.

### Typed Id

All entities are strongly typed. This means the **ID** is a unique struct defined in the project.

To create a new entity with a strongly typed id, follow this example:

```csharp

// MyEntityId.cs

[StronglyTypedId(backingType: StronglyTypedIdBackingType.Guid,

jsonConverter: StronglyTypedIdJsonConverter.NewtonsoftJson)]

public  partial  struct  MyEntityId {
}

```

```csharp

// MyEntity.cs

public  class  MyEntity : Entity<MyEntityId> {
}

```

In background, the StronglyTypedId library will create all the necessary code for you. (Don't forget to make your struct **partial**)

## Infrastructure

In this project you will find all the code relative to webapi like swagger configuration, Middleware.

## Persistence

In this project you will find all the code relative to the mapping between the entities and the database but also all the helpers for query or request the database.

### Configurations

In Configuration folder, you will find the mapping between your entities and the database. There are two base classes, one for BasicEntity and one for Entity. These base classes register the classic fields for each entity.

For example:

```csharp

// MyEntityTypeConfiguration.cs

public  class  MyEntityTypeConfiguration : EntityTypeConfiguration<MyEntity, MyEntityId> {

    protected  override  void  ConfigureEntity(EntityTypeBuilder<TEntity> builder) {

        builder.Property(p => p.MyAwesomeField)
            .HasColumnName("my_awesome_field");
    }

}

```

### TaskExtensions

In this folder, there are somes prototypes to simplify the pagination and the tracking inside services. [More documentation here](TaskExtensions.md)

## Repositories

After the configuration of the mapping for each entity, it's repository time ! You will probably need to create some repositories, just inherit from AppRepository like this example:

```csharp
public class MyEntityRepository : AppRepository<MyEntity, MyEntityId>, IMyEntityRepository
{    
    public MyEntityRepository(AppDbContext context)
            : base(context)
        {
        }
}

```

(Don't forget to create the interface !)

```csharp
public  interface  IMyEntityRepository : IRepository<MyEntity, MyEntityId>
{
}

```

## Application

Not much to do here, just inherit from *Service*. It's a very basic abstract class but needed to auto register into the dependency injection service. Feel free to improve that class.

You also have an interface *IExecutionContext* that will provide you the **UserId** of the caller.

## Tests

There are three test projects : Application, DomainSpecs, Infrastructure.
In the project Application.Tests, there is a *GenericTest* abstract class. If you have your test class inherit from *GenericTest*, you will have access to methods helping you mock all the base repositories methods.

```csharp
public  class  MyServiceTest : GenericTest<MyService>
{
}

```

Test example
```csharp
[Fact]
public async void GetProjectByIdAsync_KnownProject()
{
    var projectId = new ProjectId(Guid.NewGuid());
    const string projectName = "This is a project";
    var projects = new List<Project>
    {
        new Project
        {
            Id = projectId,
            Name = projectName
        }
    };
    MockGetByIdAsync<IProjectRepository, Project, ProjectId>(_projectRepositoryMock, projects);
    var service = BuildService();
    var result = await service.GetProjectByIdAsync(projectId);

    Assert.Equal(projectId, result.Id);
    Assert.Equal(projectName, result.Name);
}
```

The *GenericTest* contains mocks for *IUnitOfWork*, *ILogger* and *IExecutionContext* so all you need to focus on are your repositories and services.

# What do I need to do before I start coding?

## Change the solution name

Inside visual studio change the solution name to match with your service.

## Change the base namespace

Because great projects need to have a great namespace and we are not savages, you need to change the base namespace in only one place.

Open *build.props* and change the value inside **BaseNamespace**

```xml

<Project>
    <Import  Project="version.props"  />
    <PropertyGroup>
        <BaseNamespace>SP.TheGreatestService</BaseNamespace>
    </PropertyGroup>
</Project>

```

After that if you have [the best tool in the world](https://www.jetbrains.com/resharper/), you can automatically change all the namespaces in every project. (If you don't have this tool, you probably like repetitive actions so manually change the namespaces everywhere)

## Fill the README

Open the README and feel free to improve and make a good introduction to your newly created service.

## Docker-compose

It's almost done !

Open the docker-compose.override.yml file and replace template by your service name. (No upper case and be careful with _ and -)
  
# How to be sure my modification haven't blown the service up ?

Run this command:

On linux, Mac OSX and windows with powershell 7+
```bash
> docker-compose build webapi && docker-compose up webapi
```
On Windows with command line or basic powershell
```cmd
> docker-compose build webapi; docker-compose up webapi
```

If anything goes wrong, two possibilities:

First (the most common case), you forgot something or made unexpected changes. So try to understand and double check every step.

Second, after trying everything for several days, drink more coffee than you can endure. There may be a mistake in this guide. In this very rare case, you can gently ask the author of this guide for help.

# I want to create a new class library !

No problem ! Create a new class library and put it into the folder src.
After that create a file named *Directory.Build.props* next to the csproj of your project.

Copy this content into your newly file

```xml
<Project>
	<Import Project="../../build.props" />
</Project>
```

After that open with a text editor the csproj and make it like this exemple

```xml
<Project Sdk="Microsoft.NET.Sdk">

	<PropertyGroup>
		<TargetFramework>net5.0</TargetFramework>
		<RootNamespace>$(BaseNamespace).MyNewProject</RootNamespace>
	</PropertyGroup>

	<ItemGroup>
	  <None Remove="Directory.Build.props" />
	</ItemGroup>
</Project>
```
Save and close the csproj and you are good to go !

