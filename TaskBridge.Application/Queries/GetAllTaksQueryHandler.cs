using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using TaskBridge.Application.DTOs;
using TaskBridge.Application.Interfaces;

namespace TaskBridge.Application.Queries;

public class GetAllTaksQueryHandler : IRequestHandler<GetAllTasksQuery , List<TaskDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public GetAllTaksQueryHandler(IApplicationDbContext context, IConnectionMultiplexer connectionMultiplexer)
    {
        _context = context;
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<List<TaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
    { 
        var db =  _connectionMultiplexer.GetDatabase();
        var chached = await db.StringGetAsync("Tasks:all");

        if (!chached.IsNullOrEmpty)
        {
            return JsonSerializer.Deserialize<List<TaskDto>>(chached);
        }
        var allTask = await _context.Tasks.ToListAsync(cancellationToken);
        var taskDtos =  allTask.Select(task => new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Budget = task.Budget,
            Status = task.Status,
            CreatedAt = task.CreatedAt
        }).ToList(); 

        await db.StringSetAsync("Tasks:all",
            JsonSerializer.Serialize(taskDtos),
            TimeSpan.FromMinutes(10));
        return taskDtos;
    }
}