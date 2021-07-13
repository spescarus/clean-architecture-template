using Microsoft.Extensions.DependencyInjection;
using SP.SampleCleanArchitectureTemplate.Application.Base;

namespace SP.SampleCleanArchitectureTemplate.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IValidationService, ValidationService>();
            return services.Scan(scan =>
            {
                scan.FromAssemblies(typeof(ServiceCollectionExtensions).Assembly)
                    .AddClasses(classes => classes.AssignableTo(typeof(Service)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });
        }
    }
}
