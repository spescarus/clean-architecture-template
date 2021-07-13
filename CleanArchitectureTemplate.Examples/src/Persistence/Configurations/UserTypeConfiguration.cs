using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SP.SampleCleanArchitectureTemplate.Domain.Users;
using SP.SampleCleanArchitectureTemplate.Domain.Users.ValueObjects;
using SP.SampleCleanArchitectureTemplate.Persistence.Configurations.Generics;

namespace SP.SampleCleanArchitectureTemplate.Persistence.Configurations
{
    public sealed class UserTypeConfiguration : BasicEntityTypeConfiguration<User, UserId>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");


            builder.Property(p => p.UserName)
                   .HasConversion(p => p.Value, p => UserName.Create(p)
                                                             .Value)
                   .HasColumnName("username")
                   .IsRequired();

            builder.Property(p => p.Email)
                   .HasConversion(p => p.Value, p => Email.Create(p)
                                                          .Value)
                   .HasColumnName("email")
                   .IsRequired();

            builder.OwnsOne(p => p.Name, p =>
            {
                p.Property(pp => pp.First)
                 .HasColumnName("first_name")
                 .IsRequired();

                p.Property(pp => pp.Last)
                 .HasColumnName("last_name")
                 .IsRequired();
            });

            builder.OwnsOne(p => p.Address, p =>
            {
                p.Property(pp => pp.Address1)
                 .HasColumnName("address1");

                p.Property(pp => pp.City)
                 .HasColumnName("city");

                p.Property(pp => pp.Country)
                 .HasColumnName("country");
            });
        }
    }
}
