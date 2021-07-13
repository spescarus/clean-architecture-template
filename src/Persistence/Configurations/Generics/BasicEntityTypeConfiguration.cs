using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SP.CleanArchitectureTemplate.Domain.Base.Interfaces;

namespace SP.CleanArchitectureTemplate.Persistence.Configurations.Generics
{
    [ExcludeFromCodeCoverage]
    public abstract class BasicEntityTypeConfiguration<TEntity, TEntityId> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, IBasicEntity<TEntityId>
        where TEntityId : struct
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(p => p.CreatedAt)
                   .HasColumnName("created_at")
                   .IsRequired();
            builder.Property(p => p.UpdatedAt)
                   .HasColumnName("updated_at")
                   .IsRequired();
            builder.Property(p => p.DeletedAt)
                   .HasColumnName("deleted_at");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                   .HasColumnName("id")
                   .IsRequired();

            builder.HasQueryFilter(p => p.DeletedAt == null);
            ConfigureEntity(builder);
        }

        protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
    }
}
