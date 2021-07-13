using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Infrastructure.Swagger
{
    /// <summary>
    /// Swagger filter used to correct "required" information in multipart endpoints.
    /// Will only work when Required attribute is used with FromForm attribute
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class FromFormRequiredFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema       schema,
                          SchemaFilterContext context)
        {
            if (context.MemberInfo == null)
                return;

            if (context.MemberInfo.CustomAttributes.Any(x => x.AttributeType == typeof(FromFormAttribute)) &&
                context.MemberInfo.CustomAttributes.Any(x => x.AttributeType == typeof(RequiredAttribute)))
            {
                schema.Required.Add("true");
                schema.Nullable = false;
            }
        }
    }
}
