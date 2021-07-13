using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SP.SampleCleanArchitectureTemplate.Application.Exceptions;

namespace SP.SampleCleanArchitectureTemplate.Application.Base
{
    [ExcludeFromCodeCoverage]
    public class ValidationService : IValidationService
    {
        private readonly IServiceProvider _services;

        public ValidationService(IServiceProvider services)
        {
            _services = services;
        }

        public async Task ValidateAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            var validator  = _services.GetService<IValidator<TEntity>>();
            if (validator != null)
            {
                var validation = await validator.ValidateAsync(entity);
                if (!validation.IsValid)
                {
                    throw new InvalidEntityStateException<TEntity>(validation.Errors);
                }
            }
        }
    }
}
