using MediatR;
using TaskBridge.Application.DTOs;

namespace TaskBridge.Application.Queries;

public record GetAllApplicationQuery : IRequest<List<ApplicationDto>>;