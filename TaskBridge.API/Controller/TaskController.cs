using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBridge.Application.Commands;
using TaskBridge.Application.Queries;

namespace TaskBridge.Controller;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaskController(IMediator mediator)
    {
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
        var result = await _mediator.Send(new GetMyTasksQuery());
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, UpdateTaskCommand command)
    {
        var commandWithId = command with {TaskId = id};
        var result = await _mediator.Send(commandWithId);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var delete = await _mediator.Send(new DeleteTaskCommand(id));
        return Ok(delete);
    }
}