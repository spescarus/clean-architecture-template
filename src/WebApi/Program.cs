using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SP.CleanArchitectureTemplate.WebApi
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
                                                              .SetBasePath(Directory.GetCurrentDirectory())
                                                              .AddJsonFile("appsettings.json", false, true)
                                                              .AddJsonFile(
                                                                   $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                                                                   true)
                                                              .AddEnvironmentVariables()
                                                              .Build();

        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(Configuration)
                        .Enrich.FromLogContext()
                        .WriteTo.Debug()
                        .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args)
                   .Build()
                   .Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .ConfigureWebHostDefaults(webBuilder =>
                       {
                           webBuilder.UseStartup<Startup>();
                           webBuilder.UseSerilog((hostingContext,
                                                  loggerConfiguration) =>
                                                     loggerConfiguration.ReadFrom.Configuration(
                                                         hostingContext.Configuration))
                                     .CaptureStartupErrors(true);
                       });
        }
    }
}
