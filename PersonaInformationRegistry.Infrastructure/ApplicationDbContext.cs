using Microsoft.EntityFrameworkCore;
using PersonalInformationRegistry.Domain.Entities;

namespace PersonaInformationRegistry.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<Person> People { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PersonConfiguration());
        modelBuilder.ApplyConfiguration(new CredentialsConfiguration());
    }
}
