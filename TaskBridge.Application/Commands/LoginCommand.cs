using MediatR;

namespace TaskBridge.Application.Commands;

public record LoginCommand (string Email , string Password): IRequest<string>;