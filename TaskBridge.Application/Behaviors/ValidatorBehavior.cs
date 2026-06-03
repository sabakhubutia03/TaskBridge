using FluentValidation;
using FluentValidation.Results;
using MediatR;
using TaskBridge.Domain.Errors;

namespace TaskBridge.Application.Behaviors;

public class ValidatorBehavior <TRequest,TResponse> :
    IPipelineBehavior<TRequest,TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidatorBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var failures = new List<ValidationFailure>();
        foreach (var validator in _validators)
        {
            var result = await validator.ValidateAsync(request , cancellationToken);
            failures.AddRange(result.Errors);
        }

        if (failures.Any())
        {
            throw new ApiException(
                "errors/bad-request",
                "Bad Request",
                400,
                failures.First().ErrorMessage,
                "/api/task"
                );
        }

        return await next();
    }
}