using Microsoft.EntityFrameworkCore;
using TaskBridge.Application.DTOs;
using TaskBridge.Application.Interfaces;
using TaskBridge.Domain.Entity;
using TaskBridge.Domain.Enums;
using TaskBridge.Domain.Errors;

namespace TaskBridge.Application.Services;

public class TaskService : ITaskService
{
    private readonly IApplicationDbContext _context;

    public TaskService(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<TaskDto> CreateTask(TaskCreateDto dto , Guid currentUserId)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new ApiException(
                "errors/bad-request",
                "Bad Request",
                400,
                "Title cannot be empty",
                "/api/users/CreateTask"
            );


        if (string.IsNullOrWhiteSpace(dto.Description))
            throw new ApiException(
                "errors/bad-request",
                "Bad Request",
                400,
                "Description cannot be empty",
                "/api/users/CreateTask"
            );

        if (dto.Budget <=0) 
            throw new ApiException(
                "errors/bad-request",
                "Bad Request",
                400,
                "Budget cannot be negative",
                "/api/users/CreateTask"
            );
        
        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Budget = dto.Budget,
            Status = Status.Open,
            CreatedAt = DateTime.UtcNow,
            UserId = currentUserId 
        };
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Budget = task.Budget,
            Status = task.Status,
            CreatedAt = task.CreatedAt,
        };
    }

    public async Task<IEnumerable<TaskDto>> GetAllTasks()
    {
        var taskws = await _context.Tasks.ToListAsync();
        return taskws.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Budget = t.Budget,
                Status = t.Status,
                CreatedAt = t.CreatedAt
            }
        ).ToList();
    }

    public async Task<IEnumerable<TaskDto>> GetMyTasks(Guid UserId)
    {
        var task = await _context.Tasks.Where
                (t => t.UserId == UserId)
            .ToListAsync();
        return task.Select(task => new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Budget = task.Budget,
            Status = task.Status,
            CreatedAt = task.CreatedAt,
        });
    }

    public async Task<TaskDto> UpdateTask(Guid id, TaskUpdateDto dto, Guid userId)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync
            (t => t.Id == id && t.UserId == userId);
        if (task == null)
        {
            throw new ApiException(
                "errors/not-found",
                "Not Found",
                404,
                "Task with ID not found or you don't have permission.",
                "/api/task/update"
            );
        }

        if (!string.IsNullOrWhiteSpace(dto.Title))
        {
            task.Title = dto.Title;
        }

        if (!string.IsNullOrWhiteSpace(task.Description))
        {
            task.Description =  dto.Description;
        }

        if (dto.Budget > 0)
        {
            task.Budget =  dto.Budget;
        }
        
        await _context.SaveChangesAsync();
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