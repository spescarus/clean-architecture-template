using System;
using System.Diagnostics.CodeAnalysis;
using SP.SampleCleanArchitectureTemplate.Domain.Base.Interfaces;

namespace SP.SampleCleanArchitectureTemplate.Domain.Base
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
