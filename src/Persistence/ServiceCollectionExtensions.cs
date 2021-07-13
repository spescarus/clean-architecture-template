using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using SP.CleanArchitectureTemplate.Application.RepositoryInterfaces.Generics;
using SP.CleanArchitectureTemplate.Persistence.Context;
using SP.CleanArchitectureTemplate.Persistence.Repositories.Generics;

namespace SP.CleanArchitectureTemplate.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services.Scan(scan =>
            {
                scan.FromAssemblies(typeof(ServiceCollectionExtensions).Assembly)
                    .AddClasses(classes => classes.AssignableTo(typeof(IQueryRepository<,>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
                scan.FromAssemblyOf<IDbContextInterceptor>()
                    .AddClasses(classes => classes.AssignableTo<IDbContextInterceptor>())
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services,
                                                     string                  databaseConnectionString)
        {
            return services.AddDbContext<AppDbContext>(o =>
            {
                o.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
                o.UseNpgsql(databaseConnectionString);
            });
        }
    }
}
