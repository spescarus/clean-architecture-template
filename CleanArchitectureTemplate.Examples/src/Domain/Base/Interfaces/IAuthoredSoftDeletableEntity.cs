using SP.SampleCleanArchitectureTemplate.Domain.Users;

namespace SP.SampleCleanArchitectureTemplate.Domain.Base.Interfaces
{
    public interface IAuthoredSoftDeletableEntity : ISoftDeletableEntity
    {
        UserId? DeletedBy { get; set; }
    }
}
