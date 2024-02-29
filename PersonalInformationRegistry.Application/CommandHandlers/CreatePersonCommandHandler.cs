using AutoMapper;
using MediatR;
using PersonalInformationRegistry.Application.Commands;
using PersonalInformationRegistry.Domain.Entities;
using PersonalInformationRegistry.Domain.Repositories;

namespace PersonalInformationRegistry.Application.CommandHandlers;

public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, int>
{
    private readonly IPersonRepository _personRepository;
    private readonly IMapper _mapper;

    public CreatePersonCommandHandler(IPersonRepository personRepository, IMapper mapper)
    {
        _personRepository = personRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = _mapper.Map<Person>(request);

        await _personRepository.AddAsync(person);
        
        return person.Id;
    }
}