using MediatR;
using TaskBridge.Application.DTOs;
using TaskBridge.Application.Interfaces;
using TaskBridge.Domain.Entity;
using TaskBridge.Domain.Enums;
using TaskBridge.Domain.Errors;

namespace TaskBridge.Application.Commands;

public class CreateApplicationCommandHandler : 
    IRequestHandler<CreateApplicationCommand , ApplicationDto >
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateApplicationCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    public async Task<ApplicationDto> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
      
        var taskId = await _context.Tasks.FindAsync(request.TaskId);
        if (taskId == null)
        {
            throw new ApiException(
                "errors/not found",
                "Not found",
                404,
                "Task cannot found",
                "/api/users/Apply"
                );
        }

        var userId = _currentUserService.UserId();

        var apply = new Applicationn
        {
            Id = Guid.NewGuid(),
            TaskItemId = taskId.Id,
            FreelancerId = userId,
            Status = Status.Open,
            Created = DateTime.UtcNow
        };
        
         _context.Applications.Add(apply);
        await _context.SaveChangesAsync(cancellationToken);

        return new ApplicationDto
        {
            TaskId = taskId.Id,
            Status = Status.Open,
            FreelancerId = userId,
            CreatedAt = DateTime.UtcNow
        };
    }
}