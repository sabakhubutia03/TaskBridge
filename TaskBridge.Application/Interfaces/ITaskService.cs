using TaskBridge.Application.DTOs;

namespace TaskBridge.Application.Interfaces;

public interface ITaskService
{
    Task<TaskDto> CreateTask(TaskCreateDto dto , Guid currentUserId);
    Task<IEnumerable<TaskDto>>GetAllTasks();
    Task<IEnumerable<TaskDto>> GetMyTasks(Guid UserId);
    Task<TaskDto> UpdateTask(Guid id,TaskUpdateDto dto, Guid userId);
}