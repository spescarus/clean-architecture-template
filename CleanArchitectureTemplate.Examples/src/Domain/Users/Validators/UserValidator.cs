using FluentValidation;

namespace SP.SampleCleanArchitectureTemplate.Domain.Users.Validators
{
    public sealed class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(p => p.Id)
               .NotEmpty()
               .NotEqual(UserId.Empty);

            RuleFor(p => p.UserName)
               .NotEmpty();

            RuleFor(p => p.Name)
               .NotEmpty();

            RuleFor(p => p.Address)
               .NotNull()
               .SetValidator(new AddressValidator());
        }
    }
}
