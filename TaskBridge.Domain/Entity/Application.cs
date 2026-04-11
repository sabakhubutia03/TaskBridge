using TaskBridge.Domain.Enums;

namespace TaskBridge.Domain.Entity;

public class Application
{
    public Guid Id { get; set; }
    
    public Guid TaskItemId { get; set; }
    public TaskItem Task { get; set; } = null!;
    
    public Guid FreelancerId  { get; set; }
    public User Freelancer  { get; set; } = null!;
    
    public Status Status { get; set; }
    public DateTime Created { get; set; } =  DateTime.UtcNow;
    
}