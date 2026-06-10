using FluentValidation;
using TaskBridge.Application.Commands;

namespace TaskBridge.Application.Validatorss;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(e => e.Email)
            .NotEmpty().WithMessage("Email is required!")
            .EmailAddress().WithMessage("Email is invalid!");
        
        RuleFor(f => f.FirstName)
            .NotEmpty().WithMessage("First name is required!")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters!");
        
        RuleFor(l => l.LastName)
            .NotEmpty().WithMessage("Last name is required!")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters!");
        
        RuleFor(p => p.Password)
            .NotEmpty().WithMessage("Password is required!")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters!");
    }
}