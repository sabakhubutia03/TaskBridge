using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBridge.Application.Commands;
using TaskBridge.Application.DTOs;
using TaskBridge.Application.Interfaces;
using TaskBridge.Application.Queries;

namespace TaskBridge.Controller;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMediator _mediator;

    public TaskController(ITaskService taskService, ICurrentUserService currentUserService, IMediator mediator)
    {
        _taskService = taskService;
        _currentUserService = currentUserService;
        _mediator = mediator;
    }
    [HttpPost]
    public async Task<ActionResult> Create(CreateTaskCommand command)
    {
        var creta = await _mediator.Send(command);
        return Ok(creta);
    }

    [HttpGet("All")]
    public async Task<ActionResult> GetTask()
    {
        var result = await _mediator.Send(new GetAllTasksQuery());
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

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var delete = await _mediator.Send(new DeleteTaskCommand(id));
        return Ok(delete);
    }
}