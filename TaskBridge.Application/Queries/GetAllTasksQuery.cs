using MediatR;
using TaskBridge.Application.DTOs;

namespace TaskBridge.Application.Queries;

public record GetAllTasksQuery : IRequest<List<TaskDto>>;