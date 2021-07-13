using SP.SampleCleanArchitectureTemplate.Domain.Users;

namespace SP.SampleCleanArchitectureTemplate.Domain.Base.Interfaces
{
    public interface IAuthoredAuditableEntity : IAuditableEntity
    {
        UserId CreatedBy { get; set; }
        UserId UpdatedBy { get; set; }
    }
}
