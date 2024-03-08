using AutoMapper;
using MediatR;
using PersonalInformationRegistry.Application.DTOs;
using PersonalInformationRegistry.Application.Queries;
using PersonalInformationRegistry.Domain;
using PersonalInformationRegistry.Domain.Repositories;

namespace PersonalInformationRegistry.Application.QueryHandlers;

public class GetPersonQueryHandler : IRequestHandler<GetPersonQuery, PersonDto>
{
    private readonly IPersonRepository _repository;
    private readonly IMapper _mapper;

    public GetPersonQueryHandler(IPersonRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PersonDto> Handle(GetPersonQuery request, CancellationToken cancellationToken)
    {
        var person = await _repository.GetByIdAsync(request.Id) ?? throw new NotFoundException("Person not found.");
        
        return _mapper.Map<PersonDto>(person);
    }
}
