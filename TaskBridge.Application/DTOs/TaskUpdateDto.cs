namespace TaskBridge.Application.DTOs;

public class TaskUpdateDto
{
    public Guid Id { get; set; }
    public string Title {get; set;}
    public string Description {get; set;}
    public decimal Budget {get; set;}
}