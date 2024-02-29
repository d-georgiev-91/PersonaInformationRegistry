using MediatR;
using PersonalInformationRegistry.Application.DTOs;

namespace PersonalInformationRegistry.Application.CommandHandlers;

public class ListPeopleQuery : IRequest<PaginatedList<PersonDto>>
{
    public int PageNumber { get; private set; }

    public int PageSize { get; private set; }
    
    public string? SortOrder { get; private set; }
    
    public string? NameFilter { get; private set; }

    public ListPeopleQuery(int pageNumber, int pageSize, string? sortOrder, string? nameFilter)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        SortOrder = sortOrder;
        NameFilter = nameFilter;
    }
}
