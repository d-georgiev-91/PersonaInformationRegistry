using MediatR;
using Microsoft.AspNetCore.Mvc;
using PersonalInformationRegistry.Application.Commands;
using PersonalInformationRegistry.Application.DTOs;
using PersonalInformationRegistry.Application.Queries;

namespace PersonaInformationRegistry.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PeopleController : ControllerBase
{
    private readonly IMediator _mediator;

    public PeopleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreatePersonCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id }, command);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdatePersonCommand command)
    {
        command.Id = id;

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeletePersonCommand(id));
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _mediator.Send(new GetPersonQuery { Id = id });

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<PersonDto>>> Get([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string? sortOrder = null, [FromQuery] string? nameFilter = null)
    {
        if (pageNumber <= 0 || pageSize <= 0)
        {
            return BadRequest($"{nameof(pageNumber)} and {nameof(pageSize)} must be greater than zero.");
        }

        var query = new ListPeopleQuery(pageNumber, pageSize, sortOrder, nameFilter);
        var result = await _mediator.Send(query);

        return Ok(result);
    }
}

