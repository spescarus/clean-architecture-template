using System;

namespace SP.CleanArchitectureTemplate.Domain.Base.Interfaces
{
    public interface IAuditableEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}
