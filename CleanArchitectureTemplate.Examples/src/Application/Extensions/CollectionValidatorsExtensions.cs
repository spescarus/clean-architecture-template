using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace SP.SampleCleanArchitectureTemplate.Application.Extensions
{
    public static class CollectionValidatorsExtensions
    {
        public static IRuleBuilderOptions<T, IEnumerable<TCollection>> DontHaveDuplicate<T, TCollection, TProperty>(this IRuleBuilder<T, IEnumerable<TCollection>> builder,
                                                                                                                    Func<TCollection, TProperty>                   property)
        {
            return builder.Must(collection => !collection
                                              .GroupBy(property)
                                              .Any(p => p.Count() > 1));
        }

        public static IRuleBuilderOptions<T, IEnumerable<TCollection>> DontHaveSameConditionTwice<T, TCollection>(this IRuleBuilder<T, IEnumerable<TCollection>> builder,
                                                                                                                  Func<TCollection, bool>                        predicate)
        {
            return builder.Must(collection => collection
                                                 .Count(predicate) <=
                                              1);
        }
    }
}
