namespace PersonalInformationRegistry.Domain.ValueObjects;

[TestFixture]
public class EmailAddressTests
{
    private static IEnumerable<string> RestricetedTopLevelDomains => EmailAddress.RestricetedTopLevelDomains;

    [Test]
    public void Constructor_ValidEmail_CreatesInstance()
    {
        var validEmail = "example@example.com";

        var emailAddress = new EmailAddress(validEmail);

        Assert.That(emailAddress.Value, Is.EqualTo(validEmail));
    }

    [Test]
    [TestCaseSource(nameof(RestricetedTopLevelDomains))]
    public void Constructor_RestrictedDomain_ThrowsArgumentException(string topLevelDomain)
    {
        var restrictedEmail = $"example@example.{topLevelDomain}";

        Assert.Throws<ArgumentException>(() => new EmailAddress(restrictedEmail));
    }

    [Test]
    public void Constructor_EmptyString_ThrowsArgumentException()
    {
        var emptyEmail = string.Empty;

        Assert.Throws<ArgumentException>(() => new EmailAddress(emptyEmail));
    }

    [Test]
    public void ToString_ReturnsEmailValue()
    {
        var validEmail = "example@example.com";
        var emailAddress = new EmailAddress(validEmail);

        var emailValue = emailAddress.ToString();

        Assert.That(emailValue, Is.EqualTo(validEmail));
    }
}
