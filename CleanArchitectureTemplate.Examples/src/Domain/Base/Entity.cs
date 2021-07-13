using System.Diagnostics.CodeAnalysis;
using SP.SampleCleanArchitectureTemplate.Domain.Base.Interfaces;
using SP.SampleCleanArchitectureTemplate.Domain.Users;

namespace SP.SampleCleanArchitectureTemplate.Domain.Base
{
    [ExcludeFromCodeCoverage]
    public abstract class Entity<TId> : BasicEntity<TId>, IEntity<TId>
        where TId : struct
    {
        public UserId  CreatedBy { get; set; }
        public UserId  UpdatedBy { get; set; }
        public UserId? DeletedBy { get; set; }
    }
}
