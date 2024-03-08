namespace PersonalInformationRegistry.Domain.ValueObjects;

public class EmailAddress
{
    public static readonly string[] RestrictedTopLevelDomains = { "io", "zw", "ru" };

    public string Value { get; } = string.Empty;

    private EmailAddress() { }

    public EmailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email address cannot be empty.");
        }

        if (RestrictedTopLevelDomains.Any(value.EndsWith))
        {
            throw new ArgumentException("Invalid email, restricted top-level domain.");
        }

        Value = value;
    }

    public override string ToString() => Value;
}
