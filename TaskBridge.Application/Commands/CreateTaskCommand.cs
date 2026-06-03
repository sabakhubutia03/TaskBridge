using MediatR;
using TaskBridge.Application.DTOs;
using TaskBridge.Domain.Enums;

namespace TaskBridge.Application.Commands;

public record CreateTaskCommand (
    string Title, 
    string Description, 
    decimal Budget
    ) : IRequest<TaskDto>;