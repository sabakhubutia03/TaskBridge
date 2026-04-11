using TaskBridge.Domain.Enums;

namespace TaskBridge.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }
    public DateTime CreatedAt { get; set; }
}