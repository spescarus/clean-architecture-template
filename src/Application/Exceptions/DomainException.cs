using System;
using System.Diagnostics.CodeAnalysis;

namespace SP.CleanArchitectureTemplate.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public abstract class DomainException : Exception
    {
        protected DomainException([NotNull] string message,
                                  [NotNull] string errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        protected DomainException([NotNull] string    message,
                                  [NotNull] string    errorCode,
                                  [NotNull] Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        public string ErrorCode { get; }

        protected void AddError(string key,
                                string value)
        {
            Data.Add(key, value);
        }
    }
}
