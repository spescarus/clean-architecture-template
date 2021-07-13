using System;
using System.Diagnostics.CodeAnalysis;

namespace SP.CleanArchitectureTemplate.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class TechnicalException : Exception
    {
        public TechnicalException([NotNull] string message)
            : base(message)
        {
        }
    }
}
