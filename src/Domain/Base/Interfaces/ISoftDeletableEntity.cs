using System;

namespace SP.CleanArchitectureTemplate.Domain.Base.Interfaces
{
    public interface ISoftDeletableEntity
    {
        DateTime? DeletedAt { get; set; }
    }
}
