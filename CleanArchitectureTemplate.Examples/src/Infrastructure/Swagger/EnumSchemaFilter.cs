using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Infrastructure.Swagger
{
    [ExcludeFromCodeCoverage]
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema       schema,
                          SchemaFilterContext context)
        {
            var type = context.Type;
            if (type.IsEnum)
            {
                schema.Type   = "string";
                schema.Format = null;
                schema.Enum = Enum.GetNames(type)
                                  .Select(p => new OpenApiString(p))
                                  .Cast<IOpenApiAny>()
                                  .ToList();
                schema.Extensions.Add(
                    "x-ms-enum",
                    new OpenApiObject
                    {
                        ["name"]          = new OpenApiString(type.Name),
                        ["modelAsString"] = new OpenApiBoolean(true)
                    }
                );
            }
        }
    }
}
