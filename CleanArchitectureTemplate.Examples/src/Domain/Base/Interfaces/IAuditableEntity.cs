using System;

namespace SP.SampleCleanArchitectureTemplate.Domain.Base.Interfaces
{
    public interface IAuditableEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}
