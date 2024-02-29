namespace PersonalInformationRegistry.Domain.ValueObjects;

public class EmailAddress
{
    public static readonly string[] RestricetedTopLevelDomains = new[] { "io", "zw", "ru" };

    public string Value { get; private set; } = string.Empty;

    private EmailAddress() { }

    public EmailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email address cannot be empty.");
        }

        if (RestricetedTopLevelDomains.Any(value.EndsWith))
        {
            throw new ArgumentException("Invalid email, restricted top-level domain.");
        }

        Value = value;
    }

    public override string ToString() => Value;
}
