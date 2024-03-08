using Microsoft.EntityFrameworkCore;
using PersonalInformationRegistry.Domain.Entities;
using PersonalInformationRegistry.Domain.Repositories;

namespace PersonaInformationRegistry.Infrastructure.Tests;

[TestFixture]
public class PersonRepositoryTests
{
    private ApplicationDbContext _context;
    private IPersonRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new PersonRepository(_context);
    }

    private Person CreateTestPerson(string name, int age, string nationality, string pictureUrl, string email, string passwordHash)
    {
        return new Person(name, age, nationality, pictureUrl, new Credentials(email, passwordHash));
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task AddAsync_ShouldAddPerson()
    {
        var person = CreateTestPerson("Anita Ingram", 59, "Spain", "https://randomuser.me/api/portraits/women/1.jpg", "anita.ingram@example.com", "f795c1deb4dd1f17cf1bee6e2daffefe3a26ab02578238570826067438efdc36");
        await _repository.AddAsync(person);

        var addedPerson = await _context.People.FindAsync(person.Id);
        Assert.That(addedPerson, Is.Not.Null);
    }

    [Test]
    public async Task DeleteAsync_ShouldMarkPersonAsDeleted()
    {
        var person = CreateTestPerson("Jeffery Wilson", 35, "Liberia", "https://randomuser.me/api/portraits/men/2.jpg", "jeffery.wilson@example.com", "951d5ff486e5ce0a702edf6eb3e4b872b6107f59a3017c82cd4b738514291227");
        await _context.People.AddAsync(person);
        await _context.SaveChangesAsync();

        await _repository.DeleteAsync(person);

        var deletedPerson = await _context.People.FindAsync(person.Id);
        Assert.That(deletedPerson?.IsDeleted, Is.True);
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnPaginatedResults()
    {
        // Add multiple persons
        await _context.AddRangeAsync(
            CreateTestPerson("Beth Roberts", 43, "Djibouti", "https://randomuser.me/api/portraits/men/3.jpg", "beth.roberts@example.com", "aaa7240c562390eafef53976634d221e4534d48f22fa5fca24ce860bab90c005"),
            CreateTestPerson("Jordan Skinner", 99, "Norfolk Island", "https://randomuser.me/api/portraits/women/4.jpg", "jordan.skinner@example.com", "eb99026ab2dd533b23c1978825b5350162d4a79fa91b5dcdd9aebce9dc5f784a"),
            CreateTestPerson("Christopher Atkinson", 98, "Greenland", "https://randomuser.me/api/portraits/women/5.jpg", "christopher.atkinson@example.com", "2e48a2879180ab614194a976be19dac979712b831a46e2495fa20681dd07bd77")
        );
        await _context.SaveChangesAsync();

        var result = await _repository.GetAllAsync(pageNumber: 1, pageSize: 2);

        Assert.That(result.Count(), Is.EqualTo(2));
        // Additional assertions for pagination, sorting, and filtering
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnPerson()
    {
        var person = CreateTestPerson("Danielle Rivers", 80, "Kyrgyz Republic", "https://randomuser.me/api/portraits/women/6.jpg", "danielle.rivers@example.com", "d62ceb95cffedaa7581251fbfa34eac6144a9e2e53a0aaee3bb3cfd54a02e58a");
        await _context.People.AddAsync(person);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(person.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(person.Id));
    }

    [Test]
    public async Task GetByIdAsync_WithNonExistingId_ShouldReturnNull()
    {
        var result = await _repository.GetByIdAsync(999);
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetTotalCountAsync_ShouldReturnCorrectCount()
    {
        // Arrange
        await _context.AddRangeAsync(
            CreateTestPerson("Karen Sanchez", 75, "Gabon", "https://randomuser.me/api/portraits/men/99.jpg", "karen.sanchez@example.com", "9b01058c10ccb667ab33cffa70cafd52f53cd449bfa686b8f384309e7892c4aa"),
            CreateTestPerson("Karen Nelson", 12, "Gabon", "https://randomuser.me/api/portraits/men/24.jpg", "karen.nelson@example.com", "dc91e40d81bc4677f91d946216b4d26d146b5be10b2f46b08560db542b884786")
        );
        await _context.SaveChangesAsync();

        // Act
        var count = await _repository.GetTotalCountAsync();

        // Assert
        Assert.That(count, Is.EqualTo(2));
    }

    [Test]
    public async Task UpdateAsync_ShouldUpdatePerson()
    {
        var person = CreateTestPerson("Karen Nelson", 12, "Gabon", "https://randomuser.me/api/portraits/men/24.jpg", "karen.nelson@example.com", "dc91e40d81bc4677f91d946216b4d26d146b5be10b2f46b08560db542b884786");
        await _context.People.AddAsync(person);
        await _context.SaveChangesAsync();

        // Act
        person = new Person("Updated Name", person.Age, person.Nationality, person.PictureUrl, new Credentials(person.Credentials!.Email, person.Credentials.Password));
        await _repository.UpdateAsync(person);

        // Assert
        var updatedPerson = await _context.People.FindAsync(person.Id);
        Assert.That(updatedPerson?.Name, Is.EqualTo("Updated Name"));
    }
}