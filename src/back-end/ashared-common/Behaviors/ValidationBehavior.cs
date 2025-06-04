using FluentValidation;

namespace SharedCommon.ValidationBehavior;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = (
                await Task.WhenAll(
                    validators.Select(v => v.ValidateAsync(context, cancellationToken))
                )
            )
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
                throw new ValidationException(failures);
        }
        ArgumentNullException.ThrowIfNull(next);

        return await next();
    }
}
