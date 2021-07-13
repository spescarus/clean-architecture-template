using System;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Attributes
{
    /// <summary>
    /// Custom attribute used to correct the Swagger documentation
    /// when nested objects are used in multi part endpoints
    /// </summary>
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class NestedFromFormAttribute : Attribute
    {
        public string Value { get; }

        /// <summary>
        /// Attribute constructor
        /// </summary>
        /// <param name="value">Prefix used to find properties to update.
        /// Ex: "media_resource" will find "media_resource_id"
        /// and rename it "media_resource.media_resource_id"
        /// </param>
        public NestedFromFormAttribute(string value)
        {
            Value = value;
        }
    }
}
