using MediatR;
using TaskBridge.Application.DTOs;

namespace TaskBridge.Application.Commands;

public record CreateApplicationCommand (Guid TaskId):IRequest<ApplicationDto>;