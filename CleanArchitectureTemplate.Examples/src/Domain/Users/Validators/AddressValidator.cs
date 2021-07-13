using FluentValidation;
using SP.SampleCleanArchitectureTemplate.Domain.Users.ValueObjects;

namespace SP.SampleCleanArchitectureTemplate.Domain.Users.Validators
{
    public sealed class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(p => p.Address1)
               .NotEmpty();

            RuleFor(p => p.City)
               .NotEmpty();

            RuleFor(p => p.Country)
               .NotEmpty();
        }
    }
}
