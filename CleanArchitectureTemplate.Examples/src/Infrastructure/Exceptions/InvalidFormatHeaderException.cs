using System.Diagnostics.CodeAnalysis;
using SP.SampleCleanArchitectureTemplate.Application.Exceptions;

namespace Infrastructure.Exceptions
{
    [ExcludeFromCodeCoverage]
    public sealed class InvalidFormatHeaderException : TechnicalException
    {
        public InvalidFormatHeaderException(string headerKey,
                                            string format)
            : base($"The header {headerKey} is invalid. Provide a value in format {format}.")
        {
            Data.Add("HeaderName", headerKey);
        }
    }
}
