using MediatR;
using TaskBridge.Application.DTOs;

namespace TaskBridge.Application.Commands;

public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName) : IRequest<UserDto>;