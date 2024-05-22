using CleanArchitecture.Application.Features.Streamers.Command.CreateStreamer;
using CleanArchitecture.Application.Features.Streamers.Command.DeleteStreamer;
using CleanArchitecture.Application.Features.Streamers.Command.UpdateStreamer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CleanArchitecture.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class StreamerController : ControllerBase
{
    private readonly IMediator _mediator;

    public StreamerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("CreateStreamer")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType( StatusCodes.Status200OK )]
    public async Task<ActionResult<int>> CreateStreamer([FromBody] CreateStreamerCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPut("UpdateStreamer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateStreamer([FromBody] UpdateStreamerCommand command)
    {
        await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}", Name ="DeleteStreamer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> DeleteStreamer(int id)
    {
        var command = new DeleteStreamerCommand { Id = id };

        await _mediator.Send(command);

        return NoContent();
    }

}
