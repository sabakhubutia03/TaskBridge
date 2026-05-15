namespace TaskBridge.Application.Messages;

public class ApplicationSubmittedMessage
{
    public Guid ApplicationId { get; set; }
    public Guid TaskId { get; set; }
    public Guid FreelancerId { get; set; }
    public DateTime SubmittedAt { get; set; }
}