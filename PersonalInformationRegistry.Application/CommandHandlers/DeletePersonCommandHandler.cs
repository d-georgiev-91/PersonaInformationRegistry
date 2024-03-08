using MediatR;
using PersonalInformationRegistry.Application.Commands;
using PersonalInformationRegistry.Domain;
using PersonalInformationRegistry.Domain.Repositories;

namespace PersonalInformationRegistry.Application.CommandHandlers;

public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, Unit>
{
    private readonly IPersonRepository _personRepository;

    public DeletePersonCommandHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<Unit> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.Id) ?? throw new NotFoundException("Person not found.");
        await _personRepository.DeleteAsync(person);

        return Unit.Value;
    }
}