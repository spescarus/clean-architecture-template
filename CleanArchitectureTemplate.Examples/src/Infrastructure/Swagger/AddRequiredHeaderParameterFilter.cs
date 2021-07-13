using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Infrastructure.Swagger
{
    /// <summary>
    /// Add X-User-Id header information in all endpoints on Swagger
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class AddRequiredHeaderParameterFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-User-Id",
                In   = ParameterLocation.Header,
                Schema = new OpenApiSchema
                {
                    Type        = "string",
                    Description = "Must contain user id"
                },
                Required = true
            });
        }
    }
}
