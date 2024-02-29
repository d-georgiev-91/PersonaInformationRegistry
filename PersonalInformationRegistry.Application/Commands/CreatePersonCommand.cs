using MediatR;

namespace PersonalInformationRegistry.Application.Commands;

public class CreatePersonCommand : IRequest<int>
{
    public string Name { get; set; } = string.Empty;
    
    public int Age { get; set; }

    public string Nationality { get; set; } = string.Empty;

    public string PictureUrl { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
