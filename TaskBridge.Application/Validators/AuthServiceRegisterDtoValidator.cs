using FluentValidation;
using TaskBridge.Application.DTOs;

namespace TaskBridge.Application.Validators;

public class AuthServiceRegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public AuthServiceRegisterDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required!")
            .EmailAddress().WithMessage("Invalid email address!");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required!")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long!");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required!")
            .MinimumLength(2).WithMessage("First name must be at least 2 characters long!")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters!");
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required!")
            .MinimumLength(2).WithMessage("Last name must be at least 2 characters long!")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters!");
    }
}