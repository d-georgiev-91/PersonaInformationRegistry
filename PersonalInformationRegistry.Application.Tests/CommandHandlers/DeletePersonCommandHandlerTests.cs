using AutoMapper;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using PersonaInformationRegistry.Infrastructure;
using PersonalInformationRegistry.Application.CommandHandlers;
using PersonalInformationRegistry.Application.Commands;
using PersonalInformationRegistry.Domain.Entities;
using PersonalInformationRegistry.Domain.Repositories;

namespace PersonalInformationRegistry.Application.Tests.CommandHandlers;

[TestFixture]
public class DeletePersonCommandHandlerTests
{
    private IPersonRepository _repository;
    private DeletePersonCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repository = Substitute.For<IPersonRepository>();
        _handler = new DeletePersonCommandHandler(_repository);
    }

    [Test]
    public async Task Handle_ValidCommand_DeletesPerson()
    {
        var person = new Person("John Doe", 30, string.Empty, string.Empty, new Credentials(string.Empty, string.Empty));
        var command = new DeletePersonCommand(person.Id);

        _repository.GetByIdAsync(person.Id).Returns(person);

        await _handler.Handle(command, default);

        await _repository.Received(1).DeleteAsync(person);
    }

    [Test]
    public void Handle_WhenPeronNotFound_GeneratesException()
    {
        var command = new DeletePersonCommand(1);

        _repository.GetByIdAsync(Arg.Any<int>()).ReturnsNull();

        Assert.ThrowsAsync<NotFoundException>(async () => await _handler.Handle(command, default));
    }
}

