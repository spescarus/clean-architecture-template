using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SP.SampleCleanArchitectureTemplate.Domain.Base.Interfaces;

namespace SP.SampleCleanArchitectureTemplate.Persistence.Configurations.Generics
{
    [ExcludeFromCodeCoverage]
    public abstract class EntityTypeConfiguration<TEntity, TEntityId> : BasicEntityTypeConfiguration<TEntity, TEntityId>
        where TEntity : class, IEntity<TEntityId>
        where TEntityId : struct
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(p => p.CreatedBy)
                   .HasColumnName("created_by_id")
                   .IsRequired();
            builder.Property(p => p.UpdatedBy)
                   .HasColumnName("updated_by_id")
                   .IsRequired();
            builder.Property(p => p.DeletedBy)
                   .HasColumnName("deleted_by_id");
            base.Configure(builder);
        }
    }
}
