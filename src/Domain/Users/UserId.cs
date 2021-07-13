using System.Diagnostics.CodeAnalysis;

namespace SP.CleanArchitectureTemplate.Domain.Users
{
    [StronglyTypedId(backingType: StronglyTypedIdBackingType.Guid,
                     jsonConverter: StronglyTypedIdJsonConverter.SystemTextJson)]
    [ExcludeFromCodeCoverage]
    public partial struct UserId
    {
    }
}
