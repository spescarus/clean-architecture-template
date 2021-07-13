using System;

namespace SP.SampleCleanArchitectureTemplate.Domain.Base.Interfaces
{
    public interface ISoftDeletableEntity
    {
        DateTime? DeletedAt { get; set; }
    }
}
