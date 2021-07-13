using SP.CleanArchitectureTemplate.Domain.Users;

namespace SP.CleanArchitectureTemplate.Domain.Base.Interfaces
{
    public interface IAuthoredSoftDeletableEntity : ISoftDeletableEntity
    {
        UserId? DeletedBy { get; set; }
    }
}
