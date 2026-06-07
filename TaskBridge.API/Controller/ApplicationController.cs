using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBridge.Application.Commands;
using TaskBridge.Application.Interfaces;

namespace TaskBridge.Controller;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ApplicationController : ControllerBase
{
    private readonly IApplicationService _applicationService;
    private readonly IMediator _mediator;

    public ApplicationController(IApplicationService applicationService, IMediator mediator)
    {
        _applicationService = applicationService;
        _mediator = mediator;
    }


    [HttpPost]
    public async Task<ActionResult> CreateApply(CreateApplicationCommand command)
    {
        var create = await _mediator.Send(command);
        return Ok(create);
    }

    [HttpGet("My-Application")]
    public async Task<ActionResult> GetMyApp()
    {
       var myApp =  await _applicationService.GetMyApplication();
        return Ok(myApp);
    }

    [HttpGet("All-Applications")]
    public async Task<ActionResult> GetAllApplications()
    {
      var allApplication =  await _applicationService.GetTaskApplications();
        return Ok(allApplication);
    }
    
}