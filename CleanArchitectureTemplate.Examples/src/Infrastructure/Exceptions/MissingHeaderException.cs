using System.Diagnostics.CodeAnalysis;
using SP.SampleCleanArchitectureTemplate.Application.Exceptions;

namespace Infrastructure.Exceptions
{
    [ExcludeFromCodeCoverage]
    public sealed class MissingHeaderException : TechnicalException
    {
        public MissingHeaderException(string headerKey)
            : base($"This method require {headerKey} header.")
        {
            Data.Add("HeaderName", headerKey);
        }
    }
}
