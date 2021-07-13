using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SP.CleanArchitectureTemplate.Persistence.Context
{
    [ExcludeFromCodeCoverage]
    public class StronglyTypedIdValueConverter<TTypedIdValue> : ValueConverter<TTypedIdValue, Guid>
    {
        public StronglyTypedIdValueConverter(ConverterMappingHints mappingHints = null)
            : base(e => (Guid)e.GetType()
                               .GetProperty("Value")
                               .GetValue(e),
                   e => Create(e),
                   mappingHints)
        {
        }

        private static TTypedIdValue Create(Guid id)
        {
            return (TTypedIdValue)Activator.CreateInstance(typeof(TTypedIdValue), id);
        }
    }
}
