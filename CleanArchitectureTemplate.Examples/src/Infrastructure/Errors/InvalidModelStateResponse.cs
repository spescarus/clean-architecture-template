using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SP.SampleCleanArchitectureTemplate.Application.Exceptions;

namespace Infrastructure.Errors
{
    [ExcludeFromCodeCoverage]
    public class InvalidModelStateResponse : ErrorResponse
    {
        public InvalidModelStateResponse(ActionContext context)
            : base(DomainCodeErrors.InvalidModelState,
                   "The inputs supplied to the API are invalid")
        {
            PopulateErrors(context);
        }

        private void PopulateErrors(ActionContext context)
        {
            foreach (var modelState in context.ModelState)
            {
                var key    = modelState.Key;
                var errors = modelState.Value.Errors;
                if (errors.Count > 0)
                {
                    Errors.Add(key, errors.Select(p => p.ErrorMessage)
                                          .ToArray());
                }
            }
        }
    }
}
