using FluentValidation;
using TaskBridge.Application.Commands;

namespace TaskBridge.Application.Validatorss;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters"); 
        RuleFor(x => x.Budget)
            .GreaterThan(0).WithMessage("Budget must be greater than 0");
    }
}