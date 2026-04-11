using TaskBridge.Domain.Enums;

namespace TaskBridge.Domain.Entity;

public class TaskItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Budget { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; } =  DateTime.UtcNow;
    
    public Guid UserId { get; set; }

    public User User { get; set; } = null!;
}