using SP.CleanArchitectureTemplate.Domain.Users;

namespace SP.CleanArchitectureTemplate.Domain.Base.Interfaces
{
    public interface IAuthoredAuditableEntity : IAuditableEntity
    {
        UserId CreatedBy { get; set; }
        UserId UpdatedBy { get; set; }
    }
}
