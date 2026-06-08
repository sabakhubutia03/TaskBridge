using MediatR;
using TaskBridge.Application.DTOs;

namespace TaskBridge.Application.Queries;

public record GetMyApplicationQuery : IRequest<List<ApplicationDto>>;