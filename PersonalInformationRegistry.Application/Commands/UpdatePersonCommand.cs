using MediatR;

namespace PersonalInformationRegistry.Application.Commands;

public class UpdatePersonCommand : IRequest<Unit>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    public string Email { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;

    public string Nationality { get; set; } = string.Empty;

    public string PictureUrl { get; set; } = string.Empty;
}
