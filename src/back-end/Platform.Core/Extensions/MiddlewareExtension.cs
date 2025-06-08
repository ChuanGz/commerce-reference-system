using Microsoft.AspNetCore.Builder;
using Platform.Core.Validation;

namespace Platform.Core.Extensions;

public static class MiddlewareExtension
{
    public static void UsePlatformExceptionHandling(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
