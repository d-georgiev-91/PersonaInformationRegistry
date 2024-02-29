using System.Text.Json.Serialization;

namespace PersonalInformationRegistry.Domain.Entities;

public class Credentials
{
    public int PersonId { get; private set; }

    public string Email { get; private set; } = string.Empty;

    public string Password { get; private set; } = string.Empty;

    public virtual Person Person { get; private set; } = null!;

    private Credentials() { }

    public Credentials(string email) => Email = email;

    [JsonConstructor]
    public Credentials(string email, string password) : this(email)
    {
        Password = password;
    }

    public Credentials(string email, string password, Person person) : this(email, password)
    {
        Person = person;
    }
}
