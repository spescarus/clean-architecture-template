using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using Infrastructure.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SP.CleanArchitectureTemplate.Application.Exceptions;

namespace Infrastructure.Middleware
{
    [ExcludeFromCodeCoverage]
    public class ErrorHandlingMiddleware
    {
        private readonly JsonSerializerSettings           _jsonSettings;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly RequestDelegate                  _next;

        public ErrorHandlingMiddleware(RequestDelegate                  next,
                                       ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
            _next   = next;
            _jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
                Converters = new List<JsonConverter>
                {
                    new DictionaryJsonConverter()
                }
            };
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context)
                   .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex)
                   .ConfigureAwait(false);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context,
                                                Exception   exception)
        {
            var code = exception switch
            {
                ArgumentNullException => StatusCodes.Status400BadRequest,
                HttpRequestException  => StatusCodes.Status400BadRequest,
                DomainException => exception is EntityNotFoundException
                                       ? StatusCodes.Status404NotFound
                                       : StatusCodes.Status400BadRequest,
                TechnicalException          => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                _                           => StatusCodes.Status500InternalServerError
            };

            var body = JsonConvert.SerializeObject(Envelope.Error(exception.Message), _jsonSettings);

            _logger.LogError(exception, "An error occurred");

            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode  = code;
            if (!string.IsNullOrEmpty(body))
            {
                await response.WriteAsync(body);
            }
        }
    }
}
