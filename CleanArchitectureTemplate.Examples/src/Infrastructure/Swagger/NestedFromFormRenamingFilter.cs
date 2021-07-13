using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Infrastructure.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Infrastructure.Swagger
{
    /// <summary>
    /// Swagger filter used to correct documentation when
    /// nested objects are present in multipart endpoints
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class NestedFromFormRenamingFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var attributes = context.MethodInfo
                                    .CustomAttributes
                                    .Where(a => a.AttributeType == typeof(NestedFromFormAttribute))
                                    .ToList();
            if (!attributes.Any())
                return;

            var success = operation.RequestBody.Content.TryGetValue("multipart/form-data", out var operationContent);
            if (!success)
                return;

            foreach (var attribute in attributes)
            {
                var value = attribute.ConstructorArguments.First().Value?.ToString()?.Trim();
                if (string.IsNullOrWhiteSpace(value))
                    continue;
                var propertiesToChange = operationContent.Schema.Properties.Where(x => x.Key.StartsWith(value)).ToList();

                foreach (var (propertyName, schemaValue) in propertiesToChange)
                {
                    var newPropertyName = $"{value}.{propertyName}";
                    operationContent.Schema.Properties.Remove(propertyName);
                    operationContent.Schema.Properties.Add(newPropertyName, schemaValue);

                    if (operationContent.Schema.Required.Contains(propertyName))
                    {
                        operationContent.Schema.Required.Remove(propertyName);
                        operationContent.Schema.Required.Add(newPropertyName);
                    }
                }
            }
        }
    }
}
