using TaskBridge.Domain.Enums;

namespace TaskBridge.Application.DTOs;

public class ApplicationDto
{
    public Guid TaskId { get; set; } 
    public Guid FreelancerId { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
}