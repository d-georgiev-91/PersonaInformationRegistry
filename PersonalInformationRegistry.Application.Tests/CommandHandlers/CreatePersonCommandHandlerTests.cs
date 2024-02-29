using AutoMapper;
using NSubstitute;
using PersonalInformationRegistry.Application.CommandHandlers;
using PersonalInformationRegistry.Application.Commands;
using PersonalInformationRegistry.Domain.Entities;
using PersonalInformationRegistry.Domain.Repositories;

namespace PersonalInformationRegistry.Application.Tests.CommandHandlers;

[TestFixture]
public class CreatePersonCommandHandlerTests
{
    private IPersonRepository _repository;
    private IMapper _mapper;
    private CreatePersonCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repository = Substitute.For<IPersonRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreatePersonCommandHandler(_repository, _mapper);
    }

    [Test]
    public async Task Handle_ValidCommand_CreatesPerson()
    {
        var command = new CreatePersonCommand { Name = "John Doe", Age = 30, Email = "john@example.com" };
        var person = new Person("John Doe", 30, string.Empty, string.Empty, new Credentials(string.Empty, string.Empty));

        _mapper.Map<Person>(command).Returns(person);

        var actualId = await _handler.Handle(command, default);

        await _repository.Received(1).AddAsync(person);
        _mapper.Received(1).Map<Person>(command);
        Assert.That(actualId, Is.EqualTo(person.Id));
    }
}

