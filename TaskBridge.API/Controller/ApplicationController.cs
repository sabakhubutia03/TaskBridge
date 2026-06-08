using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBridge.Application.Commands;
using TaskBridge.Application.Queries;

namespace TaskBridge.Controller;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ApplicationController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApplicationController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost]
    public async Task<ActionResult> CreateApply(CreateApplicationCommand command)
    {
        var create = await _mediator.Send(command);
        return Ok(create);
    }

    [HttpGet("My-Application")]
    public async Task<ActionResult> GetMyApplication()
    {
        var myAppy = await _mediator.Send(new GetMyApplicationQuery());
        return Ok(myAppy);
    }

    [HttpGet("All-Applications")]

    public async Task<ActionResult> GetAllApplications()
    {
        var allApply = await _mediator.Send(new GetAllApplicationQuery());
        return Ok(allApply);
    }
    
}