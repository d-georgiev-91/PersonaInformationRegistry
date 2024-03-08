using NSubstitute;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PersonalInformationRegistry.Application.CommandHandlers;
using PersonalInformationRegistry.Application.Commands;
using PersonalInformationRegistry.Application.Queries;
using PersonalInformationRegistry.Application.DTOs;
using PersonaInformationRegistry.Api.Controllers;
using NSubstitute.ReturnsExtensions;

namespace PersonaInformationRegistry.Api.Tests.Controllers;

[TestFixture]
public class PeopleControllerTests
{
    private IMediator _mediator;
    private PeopleController _controller;

    [SetUp]
    public void Setup()
    {
        _mediator = Substitute.For<IMediator>();
        _controller = new PeopleController(_mediator);
    }

    [Test]
    public async Task Create_WithValidCommand_ReturnsCreatedAtActionResult()
    {
        var command = new CreatePersonCommand();
        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns(Task.FromResult(1));

        var result = await _controller.Post(command);

        Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
    }

    [Test]
    public async Task Create_WithInvalidModelState_ReturnsBadRequest()
    {
        var command = new CreatePersonCommand();
        _controller.ModelState.AddModelError("Error", "Model state is invalid");

        var result = await _controller.Post(command);

        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }


    [Test]
    public async Task UpdatePerson_WithValidCommand_ReturnsNoContentResult()
    {
        var command = new UpdatePersonCommand { Id = 1 };

        var result = await _controller.Put(1, command);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task DeletePerson_WithExistingId_ReturnsOkResult()
    {
        var expected = new Unit();
        _mediator.Send(Arg.Any<DeletePersonCommand>(), Arg.Any<CancellationToken>())
                 .Returns(Task.FromResult(expected));

        var result = await _controller.Delete(1);

        var okObjectResult = result as OkObjectResult;
        
        Assert.That(okObjectResult, Is.Not.Null);
        Assert.That(okObjectResult?.Value, Is.EqualTo(expected));
    }

    [Test]
    public async Task GetById_WithExistingId_ReturnsOkObjectResult()
    {
        _mediator.Send(Arg.Any<GetPersonQuery>(), Arg.Any<CancellationToken>())
                 .Returns(Task.FromResult(new PersonDto()));

        var result = await _controller.Get(1);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetById_WithNonExistingId_ReturnsNotFoundResult()
    {
        _mediator.Send(Arg.Any<GetPersonQuery>(), Arg.Any<CancellationToken>())
                 .ReturnsNull();

        var result = await _controller.Get(999);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task GetAll_WithValidParameters_ReturnsOkObjectResult()
    {
        var paginatedList = new PaginatedList<PersonDto>(new List<PersonDto>(), 1, 1, 10);
        _mediator.Send(Arg.Any<ListPeopleQuery>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(paginatedList));

        var result = await _controller.Get(1, 10);

        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    [TestCase(-1, 10)]
    [TestCase(1, -10)]
    public async Task GetAll_WithInvalidParameters_ReturnsBadRequestResult(int pageNumber, int pageSize)
    {
        var paginatedList = new PaginatedList<PersonDto>(new List<PersonDto>(), 1, 1, 10);
        _mediator.Send(Arg.Any<ListPeopleQuery>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(paginatedList));

        var result = await _controller.Get(pageNumber, pageSize);

        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
    }
}
