using FluentValidation;

namespace SP.SampleCleanArchitectureTemplate.Application.Extensions
{
    public static class GuidValidatorsExtensions
    {
        public static IRuleBuilderOptions<T, string> Guid<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
               .Must(value => System.Guid.TryParse(value, out _));
        }
    }
}
