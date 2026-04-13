namespace TaskBridge.Application.DTOs;

public class TaskCreateDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Budget { get; set; }
}