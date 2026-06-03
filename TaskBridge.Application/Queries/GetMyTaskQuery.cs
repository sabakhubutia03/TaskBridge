using MediatR;
using TaskBridge.Application.DTOs;

namespace TaskBridge.Application.Queries;

public record GetMyTasksQuery() : IRequest<List<TaskDto>>;