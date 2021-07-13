using System.Text.Json;
using FluentValidation;

namespace SP.CleanArchitectureTemplate.Application.Extensions
{
    public static class JsonValidatorsExtensions
    {
        public static IRuleBuilderOptions<T, string> Json<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
               .Must(value =>
                {
                    try
                    {
                        JsonDocument.Parse(value);
                        return true;
                    }
                    catch (System.Exception)
                    {
                        return false;
                    }
                });
        }
    }
}
