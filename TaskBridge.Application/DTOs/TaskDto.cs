using TaskBridge.Domain.Enums;

namespace TaskBridge.Application.DTOs;

public class TaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Budget { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
}