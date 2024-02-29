using MediatR;
using PersonalInformationRegistry.Application.DTOs;

namespace PersonalInformationRegistry.Application.Queries;

public class GetPersonQuery : IRequest<PersonDto>
{
    public int Id { get; set; }
}
