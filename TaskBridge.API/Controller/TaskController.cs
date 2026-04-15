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
    private readonly ICurrentUserService _currentUserService;

    public TaskController(ITaskService taskService, ICurrentUserService currentUserService)
    {
        _taskService = taskService;
        _currentUserService = currentUserService;
    }
    
    [HttpPost]
    public async Task<ActionResult> Post(TaskCreateDto dto)
    {
        var userId = _currentUserService.UserId();
        if(userId == Guid.Empty) return Unauthorized();
        
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
       var userId = _currentUserService.UserId();
       if (userId == Guid.Empty) return Unauthorized();
        
        var result = await _taskService.GetMyTasks(userId);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(Guid id, TaskUpdateDto dto )
    {
        var userId = _currentUserService.UserId();
        if (userId == Guid.Empty) return Unauthorized();
        
        var result = await _taskService.UpdateTask(id, dto, userId);
        return Ok(result);
        
    }
}