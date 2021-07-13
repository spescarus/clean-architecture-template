using System;
using System.Diagnostics.CodeAnalysis;
using SP.CleanArchitectureTemplate.Domain.Base.Interfaces;

namespace SP.CleanArchitectureTemplate.Domain.Base
{
    [ExcludeFromCodeCoverage]
    public abstract class BasicEntity<TId>
        : IBasicEntity<TId>
        where TId : struct
    {
        public bool IsDeleted => DeletedAt.HasValue;
        public TId  Id        { get; set; }

        public DateTime  CreatedAt { get; set; }
        public DateTime  UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
