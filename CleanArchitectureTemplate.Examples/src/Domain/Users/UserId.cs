using System.Diagnostics.CodeAnalysis;

namespace SP.SampleCleanArchitectureTemplate.Domain.Users
{
    [StronglyTypedId(backingType: StronglyTypedIdBackingType.Guid,
                     jsonConverter: StronglyTypedIdJsonConverter.SystemTextJson)]
    [ExcludeFromCodeCoverage]
    public partial struct UserId
    {
    }
}
