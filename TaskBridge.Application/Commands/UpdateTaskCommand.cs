using System.Text.Json.Serialization;
using MediatR;
using TaskBridge.Application.DTOs;

namespace TaskBridge.Application.Commands;

public record UpdateTaskCommand(
    [property:JsonIgnore]Guid TaskId,
    string Title ,
    string Description,
    decimal Budget)
    : IRequest<TaskDto>;