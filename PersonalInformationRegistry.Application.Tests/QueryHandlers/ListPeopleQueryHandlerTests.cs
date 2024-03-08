using AutoMapper;
using MediatR;
using NSubstitute;
using PersonalInformationRegistry.Application.DTOs;
using PersonalInformationRegistry.Application.Queries;
using PersonalInformationRegistry.Application.QueryHandlers;
using PersonalInformationRegistry.Domain.Entities;
using PersonalInformationRegistry.Domain.Repositories;

namespace PersonalInformationRegistry.Application.Tests.QueryHandlers;

[TestFixture]
public class ListPeopleQueryHandlerTests
{
    private IPersonRepository _repository;
    private IMapper _mapper;
    private IRequestHandler<ListPeopleQuery, PaginatedList<PersonDto>> _handler;

    [SetUp]
    public void SetUp()
    {
        _repository = Substitute.For<IPersonRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new ListPeopleQueryHandler(_repository, _mapper);
    }

    [Test]
    public async Task Handle_ValidQuery_ReturnsPaginatedListOfPersonDto()
    {
        var query = new ListPeopleQuery(1, 10, null, null);
        var people = new List<Person> { new Person("John Doe", 30, "American", "url", new Credentials("email@example.com", "password")) };

        var peopleDto = new List<PersonDto> { new PersonDto { Id = 1, Name = "John Doe" } };
        _repository.GetTotalCountAsync(Arg.Any<string>())
            .Returns(people.Count);
        _repository.GetAllAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>())
            .Returns(people);
        _mapper.Map<List<PersonDto>>(people).Returns(peopleDto);

        var result = await _handler.Handle(query, default);

        Assert.That(result, Is.InstanceOf<PaginatedList<PersonDto>>());
        Assert.That(result.Items, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task Handle_EmptyRepository_ReturnsEmptyPaginatedList()
    {
        var query = new ListPeopleQuery(1, 10, null, null);
        _repository.GetAllAsync(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>()).Returns(new List<Person>());
        _mapper.Map<List<PersonDto>>(Arg.Any<List<Person>>()).Returns(new List<PersonDto>());

        var result = await _handler.Handle(query, default);

        Assert.That(result, Is.InstanceOf<PaginatedList<PersonDto>>());
        Assert.That(result.Items, Is.Empty);
    }
}
