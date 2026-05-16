using EduFlow.Shared.Results;
using FluentValidation;
using MediatR;

namespace EduFlow.Shared.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : class
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        => _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);

        var failures = _validators
            .Select(v => v.Validate(context))
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count == 0)
            return await next(cancellationToken);

        var errorMessage = string.Join(" | ", failures.Select(f => f.ErrorMessage));
        var error = Error.Validation(errorMessage);

        var responseType = typeof(TResponse);

        if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var failMethod = responseType.GetMethod(nameof(Result<object>.Failure))!;
            return (TResponse)failMethod.Invoke(null, [error])!;
        }

        if (responseType == typeof(Result))
            return (TResponse)(object)Result.Failure(error);

        throw new ValidationException(failures);
    }
}
