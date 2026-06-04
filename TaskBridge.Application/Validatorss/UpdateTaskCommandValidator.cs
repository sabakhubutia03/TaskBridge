using FluentValidation;
using TaskBridge.Application.Commands;

namespace TaskBridge.Application.Validatorss;

public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(100).WithMessage("Title length exceeds 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Title));
        
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description length exceeds 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));
        
        RuleFor(x => x.Budget)
            .GreaterThan(0).WithMessage("Budget length exceeds 0")
            .When(x => x.Budget != 0);
    }
}