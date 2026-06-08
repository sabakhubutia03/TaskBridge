using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskBridge.Application.DTOs;
using TaskBridge.Application.Interfaces;

namespace TaskBridge.Application.Queries;

public class GetMyApplicationQueryHandler : IRequestHandler<GetMyApplicationQuery ,List<ApplicationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetMyApplicationQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    public async Task<List<ApplicationDto>> Handle(GetMyApplicationQuery request, CancellationToken cancellationToken)
    {
        var freelancerId = _currentUserService.UserId();
        var apply = await _context.Applications.Where
            (x => x.FreelancerId == freelancerId).ToListAsync(cancellationToken);
        return apply.Select(a => new ApplicationDto
        {
            FreelancerId = a.FreelancerId,
            TaskId = a.TaskItemId,
            Status = a.Status,
            CreatedAt = a.Created
        }).ToList();
    }
}