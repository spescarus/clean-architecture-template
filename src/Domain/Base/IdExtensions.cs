using System;

namespace SP.CleanArchitectureTemplate.Domain.Base
{
    public static class IdExtensions
    {
        public static TEntityId ToEntityId<TEntityId>(this string value)
            where TEntityId : struct
        {
            var guid = Guid.Parse(value);
            return (TEntityId)Activator.CreateInstance(typeof(TEntityId), guid);
        }
    }
}
