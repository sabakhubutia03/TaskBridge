using MediatR;
using TaskBridge.Application.DTOs;
using TaskBridge.Application.Interfaces;
using TaskBridge.Domain.Errors;

namespace TaskBridge.Application.Commands;

public class DeleteTaskCommandHandler :IRequestHandler<DeleteTaskCommand,TaskDto>
{
    private readonly IApplicationDbContext _context;

    public DeleteTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<TaskDto> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var delete = await _context.Tasks.FindAsync(request.Id);
        if (delete == null)
        {
            throw new ApiException(
                "Task not found",
                "Not found",
                400,
                "Task Not Found",
                "/api/Task/DeleteTask"
            );
        } 
        _context.Tasks.Remove(delete);
        await _context.SaveChangesAsync(cancellationToken);
        return new TaskDto
        {
            Id = delete.Id,
            Title = delete.Title,
            Description = delete.Description,
            Budget = delete.Budget,
            Status = delete.Status,
            CreatedAt = delete.CreatedAt
        };
    }
}
