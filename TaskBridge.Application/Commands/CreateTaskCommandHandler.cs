using MediatR;
using StackExchange.Redis;
using TaskBridge.Application.DTOs;
using TaskBridge.Application.Interfaces;
using TaskBridge.Domain.Entity;
using TaskBridge.Domain.Enums;

namespace TaskBridge.Application.Commands;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand,TaskDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public CreateTaskCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IConnectionMultiplexer connectionMultiplexer)
    {
        _context = context;
        _currentUserService = currentUserService;
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = new TaskItem
        {
            Id = Guid.NewGuid(), 
            Title = request.Title,
            Description = request.Description,
            Budget = request.Budget,
            Status = Status.Open,
            UserId = _currentUserService.UserId(),
            CreatedAt = DateTime.UtcNow
        };

        _context.Tasks.Add(task);
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