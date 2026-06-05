using MediatR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using TaskBridge.Application.DTOs;
using TaskBridge.Application.Interfaces;
using TaskBridge.Domain.Errors;

namespace TaskBridge.Application.Commands;

public class UpdateTasksCommandHandler : IRequestHandler<UpdateTaskCommand,TaskDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    
    public UpdateTasksCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IConnectionMultiplexer connectionMultiplexer)
    {
        _context = context;
        _currentUserService = currentUserService;
        _connectionMultiplexer = connectionMultiplexer;
    }
    public async Task<TaskDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var userId =  _currentUserService.UserId();
        var task = await _context.Tasks.FirstOrDefaultAsync(
            x => x.Id == request.TaskId && x.UserId == userId, cancellationToken);

        if (task == null)
        {
            throw new ApiException(
                "errors/bad-request",
                "Bad Request",
                404,
                "Task not found",
                "/api/task/update");
        }

        if (!string.IsNullOrEmpty(request.Title))
        {
            task.Title = request.Title;
        }

        if (!string.IsNullOrEmpty(request.Description))
        {
            task.Description = request.Description;
        }

        if (request.Budget > 0)
        {
            task.Budget = request.Budget;
        }

        await _context.SaveChangesAsync(cancellationToken);
        
        var db = _connectionMultiplexer.GetDatabase();
        await db.KeyDeleteAsync("Tasks:all");
        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Budget = task.Budget,
            Status = task.Status,
            CreatedAt = task.CreatedAt
        };
    }
}