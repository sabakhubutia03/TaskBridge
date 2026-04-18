using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TaskBridge.Application.DTOs;
using TaskBridge.Application.Interfaces;
using TaskBridge.Application.Validators;
using TaskBridge.Domain.Entity;
using TaskBridge.Domain.Enums;
using TaskBridge.Domain.Errors;

namespace TaskBridge.Application.Services;

public class TaskService : ITaskService
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<TaskCreateDto> _validator;
    private readonly IValidator<TaskUpdateDto> _updateValidator;

    public TaskService(
        IApplicationDbContext context,
        IValidator<TaskCreateDto> validator,
        IValidator<TaskUpdateDto> updateValidator)
    {
        _context = context;
        _validator = validator;
        _updateValidator = updateValidator;
    }
    public async Task<TaskDto> CreateTask(TaskCreateDto dto , Guid currentUserId)
    {
        var validatorResult = await _validator.ValidateAsync(dto);
        if (!validatorResult.IsValid)
        {
            var errorMessage = validatorResult.Errors.First().ErrorMessage;
            throw new ApiException(
                "errors/bad-request",
                "Bad Request",
                400,
                errorMessage,
                "/api/users/CreateTask"
            );
        }
        
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
        var validationResult = await _updateValidator.ValidateAsync(dto);
        var task = await _context.Tasks.FirstOrDefaultAsync
            (t => t.Id == id && t.UserId == userId);
        if (!validationResult.IsValid)
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

        if (!string.IsNullOrWhiteSpace(dto.Description))
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