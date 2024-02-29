using AutoMapper;
using MediatR;
using PersonalInformationRegistry.Application.Commands;
using PersonalInformationRegistry.Domain.Repositories;

namespace PersonalInformationRegistry.Application.CommandHandlers;

public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, Unit>
{
    private readonly IPersonRepository _personRepository;
    private readonly IMapper _mapper;

    public UpdatePersonCommandHandler(IPersonRepository personRepository, IMapper mapper)
    {
        _personRepository = personRepository;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.Id) ?? throw new NotFoundException("Person not found.");
        _mapper.Map(request, person);

        await _personRepository.UpdateAsync(person);

        return Unit.Value;
    }
}