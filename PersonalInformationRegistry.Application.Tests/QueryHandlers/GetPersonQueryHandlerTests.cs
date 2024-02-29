using AutoMapper;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using PersonalInformationRegistry.Application.DTOs;
using PersonalInformationRegistry.Application.Queries;
using PersonalInformationRegistry.Application.QueryHandlers;
using PersonalInformationRegistry.Domain.Entities;
using PersonalInformationRegistry.Domain.Repositories;

namespace PersonalInformationRegistry.Application.Tests.QueryHandlers;

[TestFixture]
public class GetPersonQueryHandlerTests
{
    private IPersonRepository _repository;
    private IMapper _mapper;
    private GetPersonQueryHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repository = Substitute.For<IPersonRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetPersonQueryHandler(_repository, _mapper);
    }

    [Test]
    public async Task Handle_ValidId_ReturnsPersonDto()
    {
        var personId = 1;
        var query = new GetPersonQuery { Id = personId };
        var person = new Person("John Doe", 30, "American", "url", new Credentials("email@example.com", "password"));
        var personDto = new PersonDto { Id = personId, Name = "John Doe" };

        _repository.GetByIdAsync(personId).Returns(person);
        _mapper.Map<PersonDto>(person).Returns(personDto);

        var result = await _handler.Handle(query, default);

        Assert.That(result, Is.EqualTo(personDto));
        _mapper.Received(1).Map<PersonDto>(person);
    }

    [Test]
    public void Handle_MissingId_ThrowsNotFoundException()
    {
        var query = new GetPersonQuery { Id = 1 };
        _repository.GetByIdAsync(Arg.Any<int>()).ReturnsNull();

        Assert.ThrowsAsync<NotFoundException>(async () => await _handler.Handle(query, default));
    }
}
