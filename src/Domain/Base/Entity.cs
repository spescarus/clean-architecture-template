using System.Diagnostics.CodeAnalysis;
using SP.CleanArchitectureTemplate.Domain.Base.Interfaces;
using SP.CleanArchitectureTemplate.Domain.Users;

namespace SP.CleanArchitectureTemplate.Domain.Base
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
