using AutoMapper;
using MediatR;
using PersonalInformationRegistry.Application.DTOs;
using PersonalInformationRegistry.Application.Queries;
using PersonalInformationRegistry.Domain.Repositories;

namespace PersonalInformationRegistry.Application.QueryHandlers;

public class ListPeopleQueryHandler : IRequestHandler<ListPeopleQuery, PaginatedList<PersonDto>>
{
    private readonly IPersonRepository _personRepository;
    private readonly IMapper _mapper;

    public ListPeopleQueryHandler(IPersonRepository personRepository, IMapper mapper)
    {
        _personRepository = personRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PersonDto>> Handle(ListPeopleQuery query, CancellationToken cancellationToken)
    {
        var totalRecords = await _personRepository.GetTotalCountAsync(query.NameFilter);
        var people = await _personRepository.GetAllAsync(query.PageNumber, query.PageSize, query.SortOrder, query.NameFilter);
        var peopleDtos = _mapper.Map<List<PersonDto>>(people);

        return new PaginatedList<PersonDto>(peopleDtos, totalRecords, query.PageNumber, query.PageSize);
    }
}