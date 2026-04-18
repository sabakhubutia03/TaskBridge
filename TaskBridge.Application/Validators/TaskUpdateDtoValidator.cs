using System.Data;
using FluentValidation;
using TaskBridge.Application.DTOs;

namespace TaskBridge.Application.Validators;

public class TaskUpdateDtoValidator : AbstractValidator<TaskUpdateDto>
{
    public TaskUpdateDtoValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Title));
        
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Budget)
            .GreaterThan(0).WithMessage("Budget must be greater than 0")
            .When(x => x.Budget != 0);

    }
}