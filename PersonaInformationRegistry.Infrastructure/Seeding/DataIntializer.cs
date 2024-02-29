using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PersonalInformationRegistry.Domain.Entities;
using System.Reflection;
using System.Text.Json;

namespace PersonaInformationRegistry.Infrastructure.Seeding;

public static class DataInitializer
{
    public static void SeedData(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        var environment = services.GetRequiredService<IHostEnvironment>();

        if (!environment.IsDevelopment() || context.People.Any())
        {
            return;
        }

        var assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        if (assemblyDirectory == null)
        {
            return;
        }

        var jsonData = File.ReadAllText(Path.Combine(assemblyDirectory, "people.json"));
        var people = JsonSerializer.Deserialize<IEnumerable<Person>>(jsonData, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        if (people == null)
        {
            return;
        }

        context.People.AddRange(people);
        context.SaveChanges();
    }
}
