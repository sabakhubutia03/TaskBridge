using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBridge.Application.DTOs;
using TaskBridge.Application.Interfaces;
using TaskBridge.Domain.Errors;

namespace TaskBridge.Application.Queries;

public class GetMyTasksQueryHandler : IRequestHandler<GetMyTasksQuery , List<TaskDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetMyTasksQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    public async Task<List<TaskDto>> Handle(GetMyTasksQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId();
        var myTask = await _context.Tasks.Where
            (t => t.UserId == userId)
            .ToListAsync(cancellationToken);
        if (!myTask.Any())
        {
            throw new ApiException(
                "errors/not-found",
                "Not Found",
                404,
                "No tasks found",
                "/api/task/my-tasks"
            );
        }

        return myTask.Select(myTask => new TaskDto
        {
            Id = myTask.Id,
            Title = myTask.Title,
            Budget = myTask.Budget,
            Description = myTask.Description,
            Status = myTask.Status,
            CreatedAt = myTask.CreatedAt
        }).ToList();
    }
}