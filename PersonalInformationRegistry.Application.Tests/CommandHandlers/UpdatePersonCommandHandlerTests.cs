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
public class UpdatePersonCommandHandlerTests
{
    private IPersonRepository _repository;
    private IMapper _mapper;
    private UpdatePersonCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repository = Substitute.For<IPersonRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new UpdatePersonCommandHandler(_repository, _mapper);
    }

    [Test]
    public async Task Handle_ValidCommand_UpdatesPerson()
    {
        var command = new UpdatePersonCommand { Name = "John Doe", Age = 30, Email = "john@example.com" };
        var person = new Person("John Doe", 30, string.Empty, string.Empty, new Credentials(string.Empty, string.Empty));

        _repository.GetByIdAsync(person.Id).Returns(person);

        await _handler.Handle(command, default);

        await _repository.Received(1).UpdateAsync(person);
        _mapper.Received(1).Map(command, person);
    }

    [Test]
    public void Handle_WhenPeronNotFound_GeneratesException()
    {
        var command = new UpdatePersonCommand();

        _repository.GetByIdAsync(Arg.Any<int>()).ReturnsNull();

        Assert.ThrowsAsync<NotFoundException>(async () => await _handler.Handle(command, default));
    }
}

