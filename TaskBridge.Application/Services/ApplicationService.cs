using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using TaskBridge.Application.DTOs;
using TaskBridge.Application.Interfaces;
using TaskBridge.Domain.Entity;
using TaskBridge.Domain.Enums;

namespace TaskBridge.Application.Services;

public class ApplicationService : IApplicationService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public ApplicationService(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }
    
    
    public async Task<ApplicationDto> ApplyToTask(ApplicationCreateDto dto)
    {
        if (dto.TaskId == Guid.Empty)
        {
            throw new ArgumentException("TaskId cannot be empty");
        }

        var task = await _dbContext.Tasks.FindAsync(dto.TaskId);
        if (task == null)
        {
            throw new Exception("Task not found");
        }
        
        var userId = _currentUserService.UserId();
        var application = new Applicationn 
        {
            Id = Guid.NewGuid(),
            TaskItemId = dto.TaskId,
            FreelancerId = userId ,
            Status = Status.Open,
            Created = DateTime.UtcNow
        };
        
        _dbContext.Applications.Add(application);
        await _dbContext.SaveChangesAsync();
        
        return new ApplicationDto
        {
            TaskId = application.TaskItemId,
            FreelancerId = application.FreelancerId,
            Status = application.Status,
            CreatedAt = application.Created
        };
    }

    public async Task<IEnumerable<ApplicationDto>> GetTaskApplications()
    {
        var frilancerId  = _currentUserService.UserId();
        var applications = await _dbContext.Applications.Where
            (a => a.FreelancerId == frilancerId)
            .ToListAsync();
        
        return applications.Select(a => new ApplicationDto
        {
            TaskId = a.TaskItemId,
            FreelancerId = a.FreelancerId,
            Status = a.Status,
            CreatedAt = a.Created
        });
    }

    public async Task<IEnumerable<ApplicationDto>> GetMyApplication()
    { 
        var freelancerId = _currentUserService.UserId();
        var application = await _dbContext.Applications
            .Where(a => a.FreelancerId == freelancerId)
            .ToListAsync();
        
        return application.Select(a => new ApplicationDto
            {
                TaskId = a.TaskItemId,
                FreelancerId = a.FreelancerId,
                Status = a.Status,
                CreatedAt = a.Created
            }
        );
    }
}