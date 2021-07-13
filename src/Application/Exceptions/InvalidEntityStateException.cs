using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentValidation.Results;

namespace SP.CleanArchitectureTemplate.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class InvalidEntityStateException<TEntity> : DomainException
    {
        public InvalidEntityStateException(IEnumerable<ValidationFailure> errors)
            : base($"The state of the entity {typeof(TEntity).Name} is not valid.", DomainCodeErrors.InvalidEntityState)
        {
            foreach (var error in errors)
            {
                AddError(error.PropertyName, error.ErrorMessage);
            }
        }
    }
}
