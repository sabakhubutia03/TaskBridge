using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using TaskBridge.Application.DTOs;
using TaskBridge.Application.Interfaces;

namespace TaskBridge.Application.Queries;

public class GetAllApplicationQueryHandler : IRequestHandler<GetAllApplicationQuery , List<ApplicationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IConnectionMultiplexer _redis;

    public GetAllApplicationQueryHandler(IApplicationDbContext context, IConnectionMultiplexer redis)
    {
        _context = context;
        _redis = redis;
    }
    public async Task<List<ApplicationDto>> Handle(GetAllApplicationQuery request, CancellationToken cancellationToken)
    {
        var db = _redis.GetDatabase();
        var chached = await db.StringGetAsync("Apps:all");

        if (!chached.IsNullOrEmpty)
        {
            return JsonSerializer.Deserialize<List<ApplicationDto>>(chached);
        }

        var allApply = await _context.Applications.ToListAsync(cancellationToken);
        var applysDto = allApply.Select(apply => new ApplicationDto
        {
            TaskId = apply.TaskItemId,
            FreelancerId = apply.FreelancerId,
            Status = apply.Status,
            CreatedAt = apply.Created
        }).ToList();
        
        await db.StringSetAsync("Apps:all", JsonSerializer.Serialize(applysDto),
            TimeSpan.FromMinutes(10));
        return applysDto;
    }
}