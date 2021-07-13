using System;
using System.IO;
using FluentValidation.AspNetCore;
using Infrastructure.Errors;
using Infrastructure.Json;
using Infrastructure.Middleware;
using Infrastructure.Services;
using Infrastructure.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Prometheus;
using Sharpbrake.Client;
using Sharpbrake.Http.Middleware;
using SP.CleanArchitectureTemplate.Application;
using SP.CleanArchitectureTemplate.Application.Base;
using SP.CleanArchitectureTemplate.Application.RepositoryInterfaces.Generics;
using SP.CleanArchitectureTemplate.Persistence;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SP.CleanArchitectureTemplate.WebApi
{
    public class Startup
    {
        private const     string              CorsPolicyName = "cors_policy";
        private readonly IConfiguration      _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration,
                       IWebHostEnvironment env)
        {
            _configuration = configuration;
            _environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (_environment.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
            }

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName,
                                  builder =>
                                  {
                                      (_environment.IsDevelopment()
                                           ? builder.WithOrigins($"{_configuration["Cors"]}", "http://localhost:3000")
                                           : builder.WithOrigins($"{_configuration["Cors"]}"))
                                         .AllowAnyHeader()
                                         .WithMethods("GET", "POST", "HEAD", "PUT", "OPTIONS", "DELETE")
                                         .WithExposedHeaders("Location");
                                  });
            });

            services.AddControllers()
                    .ConfigureApiBehaviorOptions(o => o.InvalidModelStateResponseFactory = context =>
                                                          new BadRequestObjectResult(
                                                              new InvalidModelStateResponse(context)))
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new SnakeCaseNamingStrategy()
                        };
                        options.SerializerSettings.Converters.Add(new DictionaryJsonConverter());
                        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    })
                    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>()
                                                 .RegisterValidatorsFromAssemblyContaining<ValidationService>());

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "VV";
                options.SubstituteApiVersionInUrl = true;
                options.SubstitutionFormat = "VV";
            });
            services.AddAutoMapper(typeof(Startup));

            services.AddDatabase(_configuration.GetConnectionString("AppDatabase"));

            services.AddRepositories();
            services.AddServices();

            services.AddHealthChecks();
            AddSwaggerService(services);

            services.AddTransient<IExecutionContext, ExecutionContext>();
        }

        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env,
                              IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseAirbrake(new AirbrakeNotifier(new AirbrakeConfig
                {
                    ProjectId = _configuration["Airbrake:ProjectId"],
                    ProjectKey = _configuration["Airbrake:ProjectKey"],
                    Environment = _configuration["Airbrake:Environment"]
                }));
                app.UseHsts();
            }

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(CorsPolicyName);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/healthz/live");
                endpoints.MapMetrics();
            });


            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                                            description.GroupName.ToUpperInvariant());
                }
            });
        }

        private static void AddSwaggerService(IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options =>
            {
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "WebApi.xml");

                options.SchemaFilter<FromFormRequiredFilter>();
                options.OperationFilter<SwaggerDefaultValues>();
                options.OperationFilter<NestedFromFormRenamingFilter>();
                options.SchemaFilter<EnumSchemaFilter>();

                options.IncludeXmlComments(xmlPath);
            });
            services.AddSwaggerGenNewtonsoftSupport();
        }
    }
}
