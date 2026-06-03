using MediatR;
using TaskBridge.Application.DTOs;

namespace TaskBridge.Application.Commands;

public record DeleteTaskCommand(Guid Id) : IRequest<TaskDto>;