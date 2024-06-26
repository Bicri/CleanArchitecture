﻿using CleanArchitecture.Application.Features.Directors.Queries.CreateDirector;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CleanArchitecture.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DirectorController : ControllerBase
{
    private IMediator _mediator;

    public DirectorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Name = "Create Director")]
    //[Authorize(Roles = "Administrator")]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    public async Task<ActionResult<int>> CreateDirector([FromBody] CreateDirectorCommand command)
    {
        return await _mediator.Send(command);
    }
}
