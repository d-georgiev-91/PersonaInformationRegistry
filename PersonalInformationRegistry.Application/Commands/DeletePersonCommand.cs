using MediatR;

namespace PersonalInformationRegistry.Application.Commands;

public class DeletePersonCommand : IRequest<Unit>
{
    public int Id { get; set; }

    public DeletePersonCommand(int id)
    {
        Id = id;
    }
}
