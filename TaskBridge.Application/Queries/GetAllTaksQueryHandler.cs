using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBridge.Application.DTOs;
using TaskBridge.Application.Interfaces;

namespace TaskBridge.Application.Queries;

public class GetAllTaksQueryHandler : IRequestHandler<GetAllTasksQuery , List<TaskDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllTaksQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
    {
        var allTask = await _context.Tasks.ToListAsync();
        return allTask.Select(task => new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Budget = task.Budget,
            Status = task.Status,
            CreatedAt = task.CreatedAt
        }).ToList();
    }
}