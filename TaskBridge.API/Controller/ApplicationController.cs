using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBridge.Application.DTOs;
using TaskBridge.Application.Interfaces;

namespace TaskBridge.Controller;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ApplicationController : ControllerBase
{
    private readonly IApplicationService _applicationService;

    public ApplicationController(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }


    [HttpPost]
    public async Task<ActionResult> CreateApplication(ApplicationCreateDto dto)
    {
        var create = await _applicationService.ApplyToTask(dto);
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