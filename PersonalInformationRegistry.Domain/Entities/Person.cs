using System.Text.Json.Serialization;

namespace PersonalInformationRegistry.Domain.Entities;

public class Person
{
    public int Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public int Age { get; private set; }

    public string Nationality { get; private set; } = string.Empty;

    public string PictureUrl { get; private set; } = string.Empty;

    public virtual Credentials? Credentials { get; private set; }

    public bool IsDeleted { get; private set; }

    private Person() { }

    [JsonConstructor]
    public Person(string name, int age, string nationality, string pictureUrl, Credentials credentials)
    {
        Name = name;
        Age = age;
        Nationality = nationality;
        PictureUrl = pictureUrl;
        Credentials = credentials;
        IsDeleted = false;
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
    }
}