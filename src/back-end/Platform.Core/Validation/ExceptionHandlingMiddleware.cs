namespace Platform.Core.Validation {
    public class ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger
    ) {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context) {
            ArgumentNullException.ThrowIfNull(context);
            try {
                await _next(context);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception) {
            context.Response.ContentType = "application/json";

            var response = exception switch {
                ValidationException validationEx => new ErrorResponse {
                    Error = "Validation failed",
                    Details = validationEx.Errors.Select(e => new {
                        field = e.PropertyName,
                        message = e.ErrorMessage,
                    }),
                },
                InvalidOperationException => new ErrorResponse { Error = exception.Message },
                _ => new ErrorResponse {
                    Error = "An error occurred while processing your request",
                },
            };

            context.Response.StatusCode = exception switch {
                ValidationException => (int)HttpStatusCode.BadRequest,
                InvalidOperationException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError,
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResponse);
        }

        private class ErrorResponse {
            public string Error { get; set; } = default!;
            public object? Details { get; set; }
        }
    }

    public class ValidationErrorResponse {
        public string Message { get; set; } = "Validation failed";
        public List<FieldError> Errors { get; set; } = [];
    }

    public class FieldError {
        public required string Field { get; set; }
        public required string Error { get; set; }
    }
}
