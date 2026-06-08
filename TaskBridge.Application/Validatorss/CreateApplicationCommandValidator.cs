using FluentValidation;
using TaskBridge.Application.Commands;

namespace TaskBridge.Application.Validatorss;

public class CreateApplicationCommandValidator :AbstractValidator<CreateApplicationCommand>
{
    public CreateApplicationCommandValidator()
    {
        RuleFor(t => t.TaskId)
            .NotEmpty().WithMessage("TaskId cannot be empty");
    }
}