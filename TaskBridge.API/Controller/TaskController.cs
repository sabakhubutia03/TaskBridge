using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBridge.Application.DTOs;
using TaskBridge.Application.Interfaces;

namespace TaskBridge.Controller;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }
    
    [HttpPost]
    public async Task<ActionResult> Post(TaskCreateDto dto)
    {
        var userClaimId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if(userClaimId == null)
            return Unauthorized();
        
        var userId = Guid.Parse(userClaimId);
        
        var result = await _taskService.CreateTask(dto, userId);
        return Ok(result);
    }

    [HttpGet("All")]
    public async Task<ActionResult> GetTask()
    {
      var result =   await _taskService.GetAllTasks();
      return Ok(result);
    }

    [HttpGet("My-Tasks")]
    public async Task<ActionResult> GetMyTasks()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized();

        var userId = Guid.Parse(userIdClaim);
        
        var result = await _taskService.GetMyTasks(userId);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(Guid id, TaskUpdateDto dto )
    {
        var userClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userClaim == null)
            return Unauthorized();
        
        var userId = Guid.Parse(userClaim);

        var result = await _taskService.UpdateTask(id, dto, userId);
        return Ok(result);
        
    }
}