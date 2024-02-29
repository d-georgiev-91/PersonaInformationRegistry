using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalInformationRegistry.Domain.Entities;

namespace PersonaInformationRegistry.Infrastructure;

public class CredentialsConfiguration : IEntityTypeConfiguration<Credentials>
{
    public void Configure(EntityTypeBuilder<Credentials> builder)
    {
        builder.HasKey(c => c.PersonId);
        builder.Property(c => c.Email).IsRequired().HasMaxLength(255);
        builder.Property(c => c.Password).IsRequired();

        builder.HasOne(p => p.Person)
               .WithOne(p => p.Credentials)
               .HasForeignKey<Credentials>(c => c.PersonId);
    }
}