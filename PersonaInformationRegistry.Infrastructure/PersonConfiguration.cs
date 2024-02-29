using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalInformationRegistry.Domain.Entities;

namespace PersonaInformationRegistry.Infrastructure;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Age).IsRequired();
        builder.Property(p => p.Nationality).IsRequired();
        builder.Property(p => p.PictureUrl).IsRequired();

        builder.HasIndex(p => p.Name);
    }
}
