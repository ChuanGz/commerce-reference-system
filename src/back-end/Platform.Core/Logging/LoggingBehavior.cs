using System.Reflection;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Platform.Core.Logging;

public sealed class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse> {
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    ) {
        if (next is null) {
            throw new ArgumentNullException(nameof(next));
        }

        var requestName = typeof(TRequest).Name;
        var requestId = GetRequestId(request);

        logger.LogInformation(
            "Handling {RequestName} {RequestId}",
            requestName,
            requestId ?? "n/a"
        );

        try {
            var response = await next();
            logger.LogInformation(
                "Handled {RequestName} {RequestId}",
                requestName,
                requestId ?? "n/a"
            );
            return response;
        }
        catch (Exception ex) {
            logger.LogError(
                ex,
                "Error handling {RequestName} {RequestId}",
                requestName,
                requestId ?? "n/a"
            );
            throw;
        }
    }

    private static string? GetRequestId(TRequest request) {
        var type = request.GetType();
        var candidates = new[] { "Id", "OrderId", "UserId", "CustomerId", "ProductId" };

        foreach (var name in candidates) {
            var prop = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            if (prop?.PropertyType == typeof(Guid)) {
                var value = (Guid?)prop.GetValue(request);
                if (value.HasValue && value.Value != Guid.Empty) {
                    return value.Value.ToString();
                }
            }
        }

        return null;
    }
}
