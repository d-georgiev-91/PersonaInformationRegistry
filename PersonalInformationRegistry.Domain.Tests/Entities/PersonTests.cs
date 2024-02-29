namespace PersonalInformationRegistry.Domain.Tests.Entities;

using NUnit.Framework;
using PersonalInformationRegistry.Domain.Entities;

[TestFixture]
public class PersonTests
{
    [Test]
    public void MarkAsDeleted_SetsIsDeletedToTrue()
    {
        var person = new Person("John Doe", 30, string.Empty, string.Empty, new Credentials(string.Empty, string.Empty));

        person.MarkAsDeleted();

        Assert.That(person.IsDeleted, Is.True);
    }
}