using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SP.SampleCleanArchitectureTemplate.Application.Exceptions;

namespace Infrastructure.Errors
{
    [ExcludeFromCodeCoverage]
    public class ErrorResponse
    {
        private   string      Code    { get; }
        private   string      Message { get; }
        protected IDictionary Errors  { get; }

        private ErrorResponse()
        {
        }

        protected ErrorResponse(string      code,
                                string      message,
                                IDictionary errors = null)
        {
            Message = message;
            Errors  = errors ?? new Dictionary<object, object>();
            Code    = code;
        }

        public static ErrorResponse FromException(Exception exception)
        {
            var type = exception.GetType();
            var code = type.IsGenericType
                           ? $"{type.BaseType?.FullName}<{string.Join(", ", type.GenericTypeArguments.Select(p => p.Name))}>"
                           : type.FullName;
            if (exception is DomainException domainException)
            {
                code = domainException.ErrorCode;
            }

            return new ErrorResponse(code, exception.Message, exception.Data);
        }
    }
}
